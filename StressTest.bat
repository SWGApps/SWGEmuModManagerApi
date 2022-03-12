set count=0
:loop
set /a count=%count%+1
curl -o tt https://localhost:7193/Mods
if %count% neq 1000 goto loop