return function()
scene.msg( 'Hello。私は、赤ちゃんです。バブバブ。' )
coroutine.yield()

scene.msg( '今日はどこから来たの？Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.' )
coroutine.yield()
 
scene.choice( 'どこから？' , 'Tokyo', 'Hakata', 'Nagoya' )
local selected = coroutine.yield()

if selected == 0 then
    scene.msg( 'へえ。教えてくれないの、、、' )
    coroutine.yield()

elseif selected == 1 then
    scene.msg( 'へぇ。東京なんだ' )
    coroutine.yield()

elseif selected == 2 then
    scene.msg( '博多から来てくれてありがとう' )
    coroutine.yield()
elseif selected == 3 then
    scene.msg( '愛知県だよね。' )
    coroutine.yield()
end
scene.msg( 'さっきも言ったけど、私は、赤ちゃんです。バブバブ。' )
coroutine.yield()
scene.msg( 'もうおわり' )
end