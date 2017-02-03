rmdir /s/q %srcDir%AndroidNative\
xcopy /S/Y %repoDir%AndroidNative\*                                           %srcDir%AndroidNative\

erase /s %srcDir%Xamarin.Forms.Core\*.cs
erase /s %srcDir%Xamarin.Forms.Core\project.json
erase /s %srcDir%Xamarin.Forms.Core\*.xaml
erase /s %srcDir%Xamarin.Forms.Core\*.txt
erase /s %srcDir%Xamarin.Forms.Core\*.jar
copy /Y    %repoDir%stubs\Xamarin.Forms.Platform.cs                           %srcDir%Xamarin.Forms\
xcopy /S/Y %repoDir%Xamarin.Forms.Core\*.cs                                   %srcDir%Xamarin.Forms\portable\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Core.Design\*.cs                            %srcDir%Xamarin.Forms\design\                                 /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform\*.cs                               %srcDir%Xamarin.Forms\portable\Platform\                      /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android\*.cs                       %srcDir%Xamarin.Forms\android\                                /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.iOS\*.cs                           %srcDir%Xamarin.Forms\ios\                                    /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT\*.cs                         %srcDir%Xamarin.Forms\winrt\                                  /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT\*.xaml                       %srcDir%Xamarin.Forms\winrt\                                  /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Phone\*.cs                   %srcDir%Xamarin.Forms\winrt\phone\                            /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Phone\*.xaml                 %srcDir%Xamarin.Forms\winrt\phone\                            /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Tablet\*.cs                  %srcDir%Xamarin.Forms\winrt\tablet\                           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.WinRT.Tablet\*.xaml                %srcDir%Xamarin.Forms\winrt\tablet\                           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.cs                           %srcDir%Xamarin.Forms\winrt\uap\                              /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.xaml                         %srcDir%Xamarin.Forms\winrt\uap\                              /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.UAP\*.json                         %srcDir%Xamarin.Forms\winrt\uap\                              /EXCLUDE:%~dp0excludeObjBin.txt
move /y %srcDir%Xamarin.Forms\winrt\phone\FormsPivot.cs                       %srcDir%Xamarin.Forms\winrt\FormsPivot.cs

erase /s %srcDir%Xamarin.Forms.Maps\*.cs
erase /s %srcDir%Xamarin.Forms.Maps\project.json
erase /s %srcDir%Xamarin.Forms.Maps\*.xaml
erase /s %srcDir%Xamarin.Forms.Maps\*Maps.targets
erase /s %srcDir%Xamarin.Forms.Maps\*Maps.props
xcopy /S/Y %repoDir%Xamarin.Forms.Maps\*.cs                                   %srcDir%Xamarin.Forms.Maps\portable\                          /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.Android\*.cs                           %srcDir%Xamarin.Forms.Maps\android\                           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.iOS\*.cs                               %srcDir%Xamarin.Forms.Maps\ios\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy   /Y %repoDir%Xamarin.Forms.Maps.UWP\*.cs                               %srcDir%Xamarin.Forms.Maps\winrt\                             /EXCLUDE:%~dp0excludeObjBin.txt
xcopy   /Y %repoDir%Xamarin.Forms.Maps.UWP\Properties\*.cs                    %srcDir%Xamarin.Forms.Maps\winrt\uap\Properties\              /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.UWP\*.xaml                             %srcDir%Xamarin.Forms.Maps\winrt\uap\                         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.UWP\*.json                             %srcDir%Xamarin.Forms.Maps\winrt\uap\                         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Phone\*.cs                       %srcDir%Xamarin.Forms.Maps\winrt\phone\                       /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Phone\*.xaml                     %srcDir%Xamarin.Forms.Maps\winrt\phone\                       /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Tablet\*.cs                      %srcDir%Xamarin.Forms.Maps\winrt\tablet\                      /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.WinRT.Tablet\*.xaml                    %srcDir%Xamarin.Forms.Maps\winrt\tablet\                      /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Maps.Design\*.cs                            %srcDir%Xamarin.Forms.Maps\design\                            /EXCLUDE:%~dp0excludeObjBin.txt
copy /Y %repoDir%.nuspec\Xamarin.Forms.Maps.targets                           %srcDir%Xamarin.Forms.Maps\build\Xamarin.Forms.Maps.targets
copy /Y %repoDir%.nuspec\Xamarin.Forms.Maps.props                             %srcDir%Xamarin.Forms.Maps\build\Xamarin.Forms.Maps.props

rmdir /s/q %srcDir%Xamarin.Forms.Maps\Resources
move %srcDir%Xamarin.Forms.Maps\android\Resources %srcDir%Xamarin.Forms.Maps\Resources

erase /s %srcDir%Xamarin.Forms.AppLinks\*.cs
erase /s %srcDir%Xamarin.Forms.AppLinks\*.txt
erase /s %srcDir%Xamarin.Forms.AppLinks\*.xml
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.AppLinks\*.cs              %srcDir%Xamarin.Forms.AppLinks\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.AppLinks\*.txt             %srcDir%Xamarin.Forms.AppLinks\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.AppLinks\*.xml             %srcDir%Xamarin.Forms.AppLinks\                               /EXCLUDE:%~dp0excludeObjBin.txt

erase /s %srcDir%Xamarin.Forms.Bindings\*.cs
erase /s %srcDir%Xamarin.Forms.Bindings\*.jar
erase /s %srcDir%Xamarin.Forms.Bindings\*.xml
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.FormsViewGroup\*.cs        %srcDir%Xamarin.Forms.Bindings\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.FormsViewGroup\*.xml       %srcDir%Xamarin.Forms.Bindings\                               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android.FormsViewGroup\*.jar       %srcDir%Xamarin.Forms.Bindings\                               /EXCLUDE:%~dp0excludeObjBin.txt

erase /s %srcDir%Xamarin.Forms.Xaml\*.cs
erase /s %srcDir%Xamarin.Forms.Xaml.BuildTasks\*.cs
xcopy /S/Y %repoDir%Xamarin.Forms.Xaml\*.cs                                   %srcDir%Xamarin.Forms.Xaml\                                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Build.Tasks\*.cs                            %srcDir%Xamarin.Forms.Xaml.BuildTasks\                        /EXCLUDE:%~dp0excludeObjBin.txt

erase /s %srcDir%Xamarin.Forms.Pages\*.cs
erase /s %srcDir%Xamarin.Forms.Pages.Azure\*.cs
xcopy /S/Y %repoDir%Xamarin.Forms.Pages\*.cs                                  %srcDir%Xamarin.Forms.Pages\                                  /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Pages.Azure\*.cs                            %srcDir%Xamarin.Forms.Pages.Azure\                            /EXCLUDE:%~dp0excludeObjBin.txt

erase /s %srcDir%Xamarin.Forms.ControlGallery\*.cs
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.xaml
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.axml
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.xml
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.jpg
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.png
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.config
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.html
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.css
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.json
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.blank
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.txt
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.plist
erase /s %srcDir%Xamarin.Forms.ControlGallery\*.base64

xcopy /S/Y %repoDir%Xamarin.Forms.CustomAttributes\*.cs                       %srcDir%Xamarin.Forms.ControlGallery\portable\                /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Controls\*.cs                               %srcDir%Xamarin.Forms.ControlGallery\portable\                /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Controls\*.xaml                             %srcDir%Xamarin.Forms.ControlGallery\portable\                /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Controls\*.jpg                              %srcDir%Xamarin.Forms.ControlGallery\portable\                /EXCLUDE:%~dp0excludeObjBin.txt

xcopy /S/Y %repoDir%Xamarin.Forms.Controls.Issues\Xamarin.Forms.Controls.Issues.Shared\*.cs                        %srcDir%Xamarin.Forms.ControlGallery\issues\                  /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Controls.Issues\Xamarin.Forms.Controls.Issues.Shared\*.xaml                      %srcDir%Xamarin.Forms.ControlGallery\issues\                  /EXCLUDE:%~dp0excludeObjBin.txt

xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.cs                  %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.xml                 %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.axml                %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.jpg                 %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.png                 %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.config              %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.html                %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.css                 %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.blank               %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Android\*.txt                 %srcDir%Xamarin.Forms.ControlGallery\android\               /EXCLUDE:%~dp0excludeObjBin.txt
copy /y %srcDir%Xamarin.Forms.ControlGallery\android\properties\MapsKey.cs.blank  %srcDir%Xamarin.Forms.ControlGallery\android\properties\MapsKey.cs

xcopy /S/Y %srcDir%Xamarin.Forms.ControlGallery\android\Resources              %srcDir%Xamarin.Forms.ControlGallery\Resources
rmdir /s/q %srcDir%Xamarin.Forms.ControlGallery\android\Resources

xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.cs                      %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.jpg                     %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.png                     %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.config                  %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.html                    %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.css                     %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.plist                   %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.iOS\*.base64                  %srcDir%Xamarin.Forms.ControlGallery\ios\                   /EXCLUDE:%~dp0excludeObjBin.txt
move /y %srcDir%Xamarin.Forms.ControlGallery\ios\info.plist                    %srcDir%Xamarin.Forms.ControlGallery\

xcopy /Y %repoDir%Xamarin.Forms.ControlGallery.WP8\*.jpg                       %srcDir%Xamarin.Forms.ControlGallery\windows\               /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /Y %repoDir%Xamarin.Forms.ControlGallery.WP8\*.png                       %srcDir%Xamarin.Forms.ControlGallery\windows\               /EXCLUDE:%~dp0excludeObjBin.txt

xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Windows\*.cs                  %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\        /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Windows\*.png                 %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\        /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Windows\*.css                 %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\        /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Windows\*.html                %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\        /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.Windows\*.xaml                %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\        /EXCLUDE:%~dp0excludeObjBin.txt

xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsUniversal\*.cs         %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsUniversal\*.png        %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsUniversal\*.css        %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsUniversal\*.html       %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsUniversal\*.xaml       %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\           /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsUniversal\*.json       %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\           /EXCLUDE:%~dp0excludeObjBin.txt

xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsPhone\*.cs             %srcDir%Xamarin.Forms.ControlGallery\windows\phone\         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsPhone\*.xaml           %srcDir%Xamarin.Forms.ControlGallery\windows\phone\         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsPhone\*.png            %srcDir%Xamarin.Forms.ControlGallery\windows\phone\         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsPhone\*.css            %srcDir%Xamarin.Forms.ControlGallery\windows\phone\         /EXCLUDE:%~dp0excludeObjBin.txt
xcopy /S/Y %repoDir%Xamarin.Forms.ControlGallery.WindowsPhone\*.html           %srcDir%Xamarin.Forms.ControlGallery\windows\phone\         /EXCLUDE:%~dp0excludeObjBin.txt

copy /y %srcDir%Xamarin.Forms.ControlGallery\windows\phone\app.xaml            %srcDir%Xamarin.Forms.ControlGallery\
copy /y %srcDir%Xamarin.Forms.ControlGallery\windows\phone\app.xaml.cs         %srcDir%Xamarin.Forms.ControlGallery\
type %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\app.xaml.cs              >> %srcDir%Xamarin.Forms.ControlGallery\app.xaml.cs
type %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\app.xaml.cs           >> %srcDir%Xamarin.Forms.ControlGallery\app.xaml.cs

move /y %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\BrokenImageSourceHandler.cs %srcDir%Xamarin.Forms.ControlGallery\windows\
copy /y %srcDir%Xamarin.Forms.ControlGallery\windows\tablet\StringProvider.cs  %srcDir%Xamarin.Forms.ControlGallery\windows\uwp\
