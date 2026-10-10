param (
    [Parameter(Mandatory=$true)]
    [string] $WcfServiceAccount
)

function Grant-CspPrivateKeyReadAccess
{
    param (
        [Parameter(Mandatory=$true)]
        [System.Security.Cryptography.RSACryptoServiceProvider] $PrivateKey,

        [Parameter(Mandatory=$true)]
        [string] $Account
    )

    $cspInfo = $PrivateKey.CspKeyContainerInfo
    $csp = New-Object System.Security.Cryptography.CspParameters(
        $cspInfo.ProviderType,
        $cspInfo.ProviderName,
        $cspInfo.KeyContainerName)

    $csp.Flags =
        [System.Security.Cryptography.CspProviderFlags]::UseExistingKey -bor
        [System.Security.Cryptography.CspProviderFlags]::UseMachineKeyStore
    $csp.CryptoKeySecurity = $cspInfo.CryptoKeySecurity
    $csp.KeyNumber = $cspInfo.KeyNumber

    $access = New-Object System.Security.AccessControl.CryptoKeyAccessRule(
        $Account,
        [System.Security.AccessControl.CryptoKeyRights]::GenericRead,
        [System.Security.AccessControl.AccessControlType]::Allow)
    $csp.CryptoKeySecurity.AddAccessRule($access)

    # Opening the existing CSP key with the updated security descriptor persists
    # the ACL change. This is the provider type used by the former net471 tool.
    $updatedKey = New-Object System.Security.Cryptography.RSACryptoServiceProvider($csp)
    $updatedKey.Dispose()
}

function Grant-CngPrivateKeyReadAccess
{
    param (
        [Parameter(Mandatory=$true)]
        [System.Security.Cryptography.RSACng] $PrivateKey,

        [Parameter(Mandatory=$true)]
        [string] $Account
    )

    $uniqueName = $PrivateKey.Key.UniqueName
    if ([string]::IsNullOrWhiteSpace($uniqueName))
    {
        throw "The CNG private key does not have a persisted unique name."
    }

    # The net10.0 CertificateGenerator uses RSA.Create and imports its PFX with
    # MachineKeySet and PersistKeySet. On Windows this creates a CNG machine key,
    # whose ACL is stored on the key file rather than in CspKeyContainerInfo.
    $keyDirectory = Join-Path $env:ProgramData "Microsoft\Crypto\Keys"
    $keyPath = Join-Path $keyDirectory $uniqueName
    if (-not (Test-Path -LiteralPath $keyPath -PathType Leaf))
    {
        throw "The persisted CNG private key file was not found at '$keyPath'."
    }

    $acl = Get-Acl -LiteralPath $keyPath
    $access = New-Object System.Security.AccessControl.FileSystemAccessRule(
        $Account,
        [System.Security.AccessControl.FileSystemRights]::Read,
        [System.Security.AccessControl.AccessControlType]::Allow)
    $acl.SetAccessRule($access)
    Set-Acl -LiteralPath $keyPath -AclObject $acl
}

try
{
    $certStores = @('My','TrustedPeople')

    foreach ($certStore in $certStores)
    {
        $certs = Get-ChildItem Cert:\LocalMachine\$certStore |
            Where-Object { $_.Issuer -like 'CN=DO_NOT_TRUST_WcfBridgeRootCA*' -and $_.HasPrivateKey }

        foreach ($cert in $certs)
        {
            $privateKey = [System.Security.Cryptography.X509Certificates.RSACertificateExtensions]::GetRSAPrivateKey($cert)
            if ($null -eq $privateKey)
            {
                throw "Certificate '$($cert.Thumbprint)' reports a private key, but its RSA private key could not be opened."
            }

            try
            {
                if ($privateKey -is [System.Security.Cryptography.RSACng])
                {
                    Grant-CngPrivateKeyReadAccess -PrivateKey $privateKey -Account $WcfServiceAccount
                }
                elseif ($privateKey -is [System.Security.Cryptography.RSACryptoServiceProvider])
                {
                    Grant-CspPrivateKeyReadAccess -PrivateKey $privateKey -Account $WcfServiceAccount
                }
                else
                {
                    throw "Certificate '$($cert.Thumbprint)' uses unsupported RSA private key type '$($privateKey.GetType().FullName)'."
                }
            }
            finally
            {
                $privateKey.Dispose()
            }
        }
    }
    exit 0
}
catch
{
    Write-Error "Failed to grant '$WcfServiceAccount' read access to a WCF test certificate private key: $($_.Exception.Message)"
    exit 1
}
