xcopy /S/Y %repoDir%Xamarin.Forms.Core\*.cs             %srcDir%Xamarin.Forms.Core\portable\
xcopy /S/Y %repoDir%Xamarin.Forms.Platform\*.cs         %srcDir%Xamarin.Forms.Core\portable\Platform\ /EXCLUDE:excludeProperties.txt
xcopy /S/Y %repoDir%Xamarin.Forms.Platform.Android\*.cs %srcDir%Xamarin.Forms.Core\android\           /EXCLUDE:excludeProperties.txt
copy /Y    %repoDir%stubs\Xamarin.Forms.Platform.cs     %srcDir%Xamarin.Forms.Core\