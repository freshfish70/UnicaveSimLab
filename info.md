# info
## Start

The project runs in a single instance of unity for each cluster of projectors (HVL-IG01, HVL-IG02, HVL-IG03)

Executa parameters
``` 
-screen-fullscreen 0 -popupwindow -screen-width 4800 -screen-height 1920 -adapter 0
```
Description
- -screen-fullscreen 0 : Override fullscreen settings and disable fullscreen
- -popupWindow : makes the window borderless
- -screen-width 4800 : Sets the window width to 4800px, total resolution(width) of 4 projectors
- -screen-height 1920 : Sets the window height to 1920px, resolutiuon height of the projectors.
- -adapter 0 : Sets the start window to be on the first display
