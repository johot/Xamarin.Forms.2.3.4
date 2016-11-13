erase /s %srcDir%Xamarin.Forms.Core\*.cs
erase /s %srcDir%Xamarin.Forms.Core\*.json
erase /s %srcDir%Xamarin.Forms.Core\*.xaml

copy /Y    %repoDir%stubs\Xamarin.Forms.Platform.cs              %srcDir%Xamarin.Forms.Core\

xcopy /S/Y %repoDir%Xamarin.Forms.Core\*.cs                      %srcDir%Xamarin.Forms.Core\portable\
xcopy /S/Y %repoDir%Xamarin.Forms.Platform\*.cs                  %srcDir%Xamarin.Forms.Core\portable\Platform\   /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android\*.cs          %srcDir%Xamarin.Forms.Core\android\             /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.iOS\*.cs              %srcDir%Xamarin.Forms.Core\ios\                 /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT\*.cs            %srcDir%Xamarin.Forms.Core\winrt\               /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT\*.xaml          %srcDir%Xamarin.Forms.Core\winrt\               /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Phone\*.cs      %srcDir%Xamarin.Forms.Core\winrt\phone\         /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Phone\*.xaml    %srcDir%Xamarin.Forms.Core\winrt\phone\         /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Tablet\*.cs     %srcDir%Xamarin.Forms.Core\winrt\tablet\        /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Tablet\*.xaml   %srcDir%Xamarin.Forms.Core\winrt\tablet\        /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.cs              %srcDir%Xamarin.Forms.Core\winrt\uap\           /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.xaml            %srcDir%Xamarin.Forms.Core\winrt\uap\           /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.json            %srcDir%Xamarin.Forms.Core\winrt\uap\           /EXCLUDE:excludeProperties.txt
