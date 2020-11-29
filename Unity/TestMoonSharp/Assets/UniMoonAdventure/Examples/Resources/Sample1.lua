return function()
scene.msg( 'Hello。私は、赤ちゃんです。バブバブ。' )
coroutine.yield()

scene.msg( '今日はどこから来たの？' )
coroutine.yield()
 
scene.choice( 'どこから？' , 'Tokyo', 'Hakata', 'Nagoya' )
local selected = coroutine.yield()

if selected == 0 then
    scene.msg( 'へえ。地元なんだ' )
    coroutine.yield()

elseif selected == 1 then
    scene.msg( 'へぇ。ようこそ。いらっしゃい' )
    coroutine.yield()

elseif selected == 2 then
    scene.msg( '遠いところからありがとう' )
    coroutine.yield()
elseif selected == 3 then
    scene.msg( 'そんなこと言わないで' )
    coroutine.yield()
end
scene.msg( 'さっきも言ったけど、私は、赤ちゃんです。バブバブ。' )
coroutine.yield()
scene.msg( 'もうおわり' )
end