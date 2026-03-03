BeforeAll {
    # Load the function under test
    . (Join-Path $PSScriptRoot '../Lib-BackupRegistry.ps1')
}

Describe 'Backup-Registry' {
    BeforeEach {
        # Prevent filesystem and registry access
        Mock -CommandName reg
        Mock -CommandName New-Item
    }

    It 'returns a path under OneDrive when available' {
        $env:OneDrive   = 'C:\Users\Test\OneDrive'
        $env:USERPROFILE = 'C:\Users\Test'

        Mock -CommandName Test-Path { $true }

        $result = Backup-Registry -keys 'HKCU:\Software\Test' -label 'Test'

        $result | Should -BeLike "$env:OneDrive\\RegistryBackups\\*"
    }

    It 'falls back to Documents when OneDrive is absent' {
        Remove-Item Env:OneDrive -ErrorAction SilentlyContinue
        $env:USERPROFILE = 'C:\Users\Test'

        Mock -CommandName Test-Path { $false }

        $result = Backup-Registry -keys 'HKCU:\Software\Test' -label 'Test'

        $result | Should -BeLike "$env:USERPROFILE\\Documents\\RegistryBackups\\*"
    }
}
