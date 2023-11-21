$template = @"
///<summary>{0}</summary>
public static readonly string {1} = "{2}";
"@

$lines = Get-Content .\InterfaceNames.txt

$replace = @{
    'Vanilla:' = '';
    ' ' =        '';
    '/' =        '';
}

foreach ($c in $lines) {
    $s = $c -Split '\t'
    $name = $s[0]

    foreach ($r in $replace.Keys) {
        $name = $name.Replace($r, $replace[$r])
    }

    $built = [string]::Format($template, $s[1], $name, $s[0])
    
    Write-Host $built
    Write-Host
}