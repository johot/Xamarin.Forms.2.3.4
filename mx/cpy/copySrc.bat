erase /s %srcDir%Xamarin.Forms.Core\*.cs
erase /s %srcDir%Xamarin.Forms.Core\*.json
erase /s %srcDir%Xamarin.Forms.Core\*.xaml

copy /Y    %repoDir%stubs\Xamarin.Forms.Platform.cs                           %srcDir%Xamarin.Forms.Core\

xcopy /S/Y %repoDir%Xamarin.Forms.Core\*.cs                                   %srcDir%Xamarin.Forms.Core\portable\
xcopy /S/Y %repoDir%Xamarin.Forms.Platform\*.cs                               %srcDir%Xamarin.Forms.Core\portable\Platform\                 /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android\*.cs                       %srcDir%Xamarin.Forms.Core\android\                           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.iOS\*.cs                           %srcDir%Xamarin.Forms.Core\ios\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT\*.cs                         %srcDir%Xamarin.Forms.Core\winrt\                             /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT\*.xaml                       %srcDir%Xamarin.Forms.Core\winrt\                             /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Phone\*.cs                   %srcDir%Xamarin.Forms.Core\winrt\phone\                       /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Phone\*.xaml                 %srcDir%Xamarin.Forms.Core\winrt\phone\                       /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Tablet\*.cs                  %srcDir%Xamarin.Forms.Core\winrt\tablet\                      /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Tablet\*.xaml                %srcDir%Xamarin.Forms.Core\winrt\tablet\                      /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.cs                           %srcDir%Xamarin.Forms.Core\winrt\uap\                         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.xaml                         %srcDir%Xamarin.Forms.Core\winrt\uap\                         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.json                         %srcDir%Xamarin.Forms.Core\winrt\uap\                         /EXCLUDE:%~dp0excludeObjBin.txt

xcopy /S/Y %repoDir%Xamarin.Forms.Maps\*.cs                                   %srcDir%Xamarin.Forms.Maps\portable\                          /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.Android\*.cs                           %srcDir%Xamarin.Forms.Maps\android\                           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.iOS\*.cs                               %srcDir%Xamarin.Forms.Maps\ios\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.UWP\*.cs                               %srcDir%Xamarin.Forms.Maps\winrt\                             /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.UWP\*.xaml                             %srcDir%Xamarin.Forms.Maps\winrt\uap\                         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.UWP\*.json                             %srcDir%Xamarin.Forms.Maps\winrt\uap\                         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Phone\*.cs                       %srcDir%Xamarin.Forms.Maps\winrt\phone\                       /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Phone\*.xaml                     %srcDir%Xamarin.Forms.Maps\winrt\phone\                       /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Tablet\*.cs                      %srcDir%Xamarin.Forms.Maps\winrt\tablet\                      /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Tablet\*.xaml                    %srcDir%Xamarin.Forms.Maps\winrt\tablet\                      /EXCLUDE:%~dp0excludeObjBin.txt

xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.FormsViewGroup\*.xml       %srcDir%Xamarin.Forms.Platform.Android.FormsViewGroup\        /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.FormsViewGroup\*.jar       %srcDir%Xamarin.Forms.Platform.Android.FormsViewGroup\        /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.FormsViewGroup\*.cs        %srcDir%Xamarin.Forms.Platform.Android.FormsViewGroup\        /EXCLUDE:%~dp0excludeObjBin.txt
