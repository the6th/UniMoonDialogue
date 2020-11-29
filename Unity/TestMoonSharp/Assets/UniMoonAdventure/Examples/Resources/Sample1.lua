return function()
scene.msg( 'Hello。私は、赤ちゃんです。バブバブ。' )
coroutine.yield()

scene.msg( '今日はどこから来たの？' )
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