powershell -ExecutionPolicy Bypass -Command {
    $ProjectDir = $PSScriptRoot
    if (-not $ProjectDir) { $ProjectDir = (Get-Location).Path }
    $PackagesDir = Join-Path $ProjectDir "..\packages"
    $DllDir = Join-Path $ProjectDir "dll"
    New-Item -ItemType Directory -Force -Path $DllDir | Out-Null
    $Mappings = @(
        "SharpDX.4.2.0\lib\net45\SharpDX.dll",
        "SharpDX.DirectInput.4.2.0\lib\net45\SharpDX.DirectInput.dll",
        "SharpDX.XInput.4.2.0\lib\net45\SharpDX.XInput.dll",
        "SharpDX.Direct2D1.4.2.0\lib\net45\SharpDX.Direct2D1.dll",
        "SharpDX.DXGI.4.2.0\lib\net45\SharpDX.DXGI.dll",
        "SharpDX.Mathematics.4.2.0\lib\net45\SharpDX.Mathematics.dll",
        "Concentus.2.2.2\lib\net452\Concentus.dll",
        "Concentus.Oggfile.1.0.7\lib\net452\Concentus.Oggfile.dll",
        "FirebaseDatabase.net.5.0.0\lib\netstandard2.0\Firebase.dll",
        "FirebaseAuthentication.net.4.1.0\lib\netstandard2.0\Firebase.Auth.dll",
        "LiteDB.5.0.17\lib\net45\LiteDB.dll",
        "Microsoft.Win32.Registry.4.7.0\lib\net461\Microsoft.Win32.Registry.dll",
        "Mono.Nat.3.0.4\lib\netstandard2.0\Mono.Nat.dll",
        "NAudio.2.3.0\lib\net472\NAudio.dll",
        "NAudio.Asio.2.3.0\lib\netstandard2.0\NAudio.Asio.dll",
        "NAudio.Core.2.3.0\lib\netstandard2.0\NAudio.Core.dll",
        "NAudio.Midi.2.3.0\lib\netstandard2.0\NAudio.Midi.dll",
        "NAudio.Wasapi.2.3.0\lib\netstandard2.0\NAudio.Wasapi.dll",
        "NAudio.WinForms.2.3.0\lib\net472\NAudio.WinForms.dll",
        "NAudio.WinMM.2.3.0\lib\netstandard2.0\NAudio.WinMM.dll",
        "Newtonsoft.Json.13.0.3\lib\net45\Newtonsoft.Json.dll",
        "System.Buffers.4.5.1\lib\net461\System.Buffers.dll",
        "System.Memory.4.5.5\lib\net461\System.Memory.dll",
        "System.Numerics.Vectors.4.5.0\lib\net46\System.Numerics.Vectors.dll",
        "System.Reactive.6.0.0\lib\net472\System.Reactive.dll",
        "System.Resources.Extensions.8.0.0\lib\net462\System.Resources.Extensions.dll",
        "System.Runtime.CompilerServices.Unsafe.4.5.3\lib\net461\System.Runtime.CompilerServices.Unsafe.dll",
        "System.Security.AccessControl.4.7.0\lib\net461\System.Security.AccessControl.dll",
        "System.Security.Principal.Windows.4.7.0\lib\net461\System.Security.Principal.Windows.dll",
        "System.Threading.Tasks.Extensions.4.5.4\lib\net461\System.Threading.Tasks.Extensions.dll"
    )
    $ok = 0; $ng = 0
    foreach ($rel in $Mappings) {
        $src = Join-Path $PackagesDir $rel
        $dst = Join-Path $DllDir (Split-Path $rel -Leaf)
        if (Test-Path $src) {
            Copy-Item $src $dst -Force
            Write-Host "  [OK] $(Split-Path $rel -Leaf)"
            $ok++
        } else {
            Write-Host "  [NG] Not found: $src"
            $ng++
        }
    }
    Write-Host "Done: $ok OK, $ng NG"
    Write-Host "DLL folder: $DllDir"
}
