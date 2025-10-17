$templateFull = @"
///<summary>{0}</summary>
public static readonly string {1} = "{2}";
"@
$templateLnk = 'public static readonly string n{0:00}_{1} = {1};'

$lines = Get-Content .\InterfaceNames.txt

$replace = @{
    'Vanilla:' = '';
    ' ' =        '';
    '/' =        '';
}

$index = 0;

foreach ($c in $lines) {
    $s = $c -Split '\t'
    $name = $s[0]

    foreach ($r in $replace.Keys) {
        $name = $name.Replace($r, $replace[$r])
    }

    $built = [string]::Format($templateFull, $s[1], $name, $s[0])
    $lnk = [string]::Format($templateLnk, $index, $name)

    Write-Host $built
    Write-Host $lnk

    Write-Host
    $index++
}