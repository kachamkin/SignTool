// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"

#pragma comment(lib, "crypt32.lib")

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

extern "C" __declspec(dllexport) HRESULT SignApp(BYTE* pbSigningCertContext, DWORD cbBytes, LPCWSTR packageFilePath, PCWSTR timestampUrl, BOOL isSigningAppx)
{
	HRESULT hr = S_OK;

	CRYPT_DATA_BLOB cb{};
	cb.pbData = pbSigningCertContext;
	cb.cbData = cbBytes;

	HCERTSTORE hSystemStore = PFXImportCertStore(
		&cb,
		NULL,
		PKCS12_ALLOW_OVERWRITE_KEY | CRYPT_EXPORTABLE);

	if (!hSystemStore)
		return E_FAIL;

	PCCERT_CONTEXT signingCertContext = CertFindCertificateInStore(
		hSystemStore,
		X509_ASN_ENCODING | PKCS_7_ASN_ENCODING,
		0,
		CERT_FIND_ANY,
		NULL,
		NULL);
	
	if (!signingCertContext)
		return E_FAIL;

	// Initialize the parameters for SignerSignEx2
	DWORD signerIndex = 0;

	SIGNER_FILE_INFO fileInfo = {};
	fileInfo.cbSize = sizeof(SIGNER_FILE_INFO);
	fileInfo.pwszFileName = packageFilePath;

	SIGNER_SUBJECT_INFO subjectInfo = {};
	subjectInfo.cbSize = sizeof(SIGNER_SUBJECT_INFO);
	subjectInfo.pdwIndex = &signerIndex;
	subjectInfo.dwSubjectChoice = SIGNER_SUBJECT_FILE;
	subjectInfo.pSignerFileInfo = &fileInfo;

	SIGNER_CERT_STORE_INFO certStoreInfo = {};
	certStoreInfo.cbSize = sizeof(SIGNER_CERT_STORE_INFO);
	certStoreInfo.dwCertPolicy = SIGNER_CERT_POLICY_CHAIN_NO_ROOT;
	certStoreInfo.pSigningCert = signingCertContext;

	SIGNER_CERT cert = {};
	cert.cbSize = sizeof(SIGNER_CERT);
	cert.dwCertChoice = SIGNER_CERT_STORE;
	cert.pCertStoreInfo = &certStoreInfo;

	SIGNER_SIGNATURE_INFO signatureInfo = {};
	signatureInfo.cbSize = sizeof(SIGNER_SIGNATURE_INFO);
	signatureInfo.algidHash = CALG_SHA_256;
	signatureInfo.dwAttrChoice = SIGNER_NO_ATTR;

	SIGNER_SIGN_EX2_PARAMS signerParams = {};
	signerParams.pSubjectInfo = &subjectInfo;
	signerParams.pSigningCert = &cert;
	signerParams.pSignatureInfo = &signatureInfo;
	signerParams.dwTimestampFlags = SIGNER_TIMESTAMP_AUTHENTICODE;
	signerParams.pwszTimestampURL = timestampUrl;

	APPX_SIP_CLIENT_DATA sipClientData;
	if (isSigningAppx)
	{
		// we only use this when signing appx packages
		sipClientData = {};
		sipClientData.pSignerParams = &signerParams;
		signerParams.pSipData = &sipClientData;
	}

	// Type definition for invoking SignerSignEx2 via GetProcAddress
	typedef HRESULT(WINAPI* SignerSignEx2Function)(
		DWORD,
		PSIGNER_SUBJECT_INFO,
		PSIGNER_CERT,
		PSIGNER_SIGNATURE_INFO,
		PSIGNER_PROVIDER_INFO,
		DWORD,
		PCSTR,
		PCWSTR,
		PCRYPT_ATTRIBUTES,
		PVOID,
		PSIGNER_CONTEXT*,
		PVOID,
		PVOID);

	// Load the SignerSignEx2 function from MSSign32.dll
	HMODULE msSignModule = LoadLibraryEx(
		L"MSSign32.dll",
		NULL,
		LOAD_LIBRARY_SEARCH_SYSTEM32);

	if (msSignModule)
	{
		SignerSignEx2Function SignerSignEx2 = reinterpret_cast<SignerSignEx2Function>(
			GetProcAddress(msSignModule, "SignerSignEx2"));
		if (SignerSignEx2)
		{
			hr = SignerSignEx2(
				signerParams.dwFlags,
				signerParams.pSubjectInfo,
				signerParams.pSigningCert,
				signerParams.pSignatureInfo,
				signerParams.pProviderInfo,
				signerParams.dwTimestampFlags,
				signerParams.pszAlgorithmOid,
				signerParams.pwszTimestampURL,
				signerParams.pCryptAttrs,
				signerParams.pSipData,
				signerParams.pSignerContext,
				signerParams.pCryptoPolicy,
				signerParams.pReserved);
		}
		else
		{
			DWORD lastError = GetLastError();
			hr = HRESULT_FROM_WIN32(lastError);
		}

		FreeLibrary(msSignModule);
	}
	else
	{
		DWORD lastError = GetLastError();
		hr = HRESULT_FROM_WIN32(lastError);
	}

	if (isSigningAppx && sipClientData.pAppxSipState)
	{
		sipClientData.pAppxSipState->Release();
	}

	CertFreeCertificateContext(signingCertContext);
	CertCloseStore(hSystemStore, CERT_CLOSE_STORE_CHECK_FLAG);

	return hr;
}

