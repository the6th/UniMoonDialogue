return function()
scene.msg( 'Hello。私は、<color=green>赤ちゃんY</color>です。\r\n<size=30>バブバブ。</size>' )
coroutine.yield()

scene.msg( '汎用ライブラリって他のOpen Source組み込みづらいですよね？' )
coroutine.yield()
 
scene.choice( 'どう思う？' , 'はい', 'いいえ' )
local selected = coroutine.yield()

if selected == 0 then
    scene.msg( 'へえ。教えてくれないの、、、' )
    coroutine.yield()

elseif selected == 1 then
    scene.msg( 'やっぱりそうですよね。UniTaskとか入れたいけど、それに依存するのもあれなので、、' )
    coroutine.yield()
    scene.msg( 'UI 絡まなければいいんですけど、、' )
    coroutine.yield()

elseif selected == 2 then
    scene.msg( 'なるほど、強いですね' )
    coroutine.yield()
end
scene.msg( 'さっきも言ったけど、私は、赤ちゃんです。バブバブ。' )
coroutine.yield()
scene.msg( 'ご清聴ありがとうございました' )
end