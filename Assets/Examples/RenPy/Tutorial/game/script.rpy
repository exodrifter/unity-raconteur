
# This is the working script for the Unity package, Raconteur, designed to integrate Ren'Py directly and seamlessly into Unity. 
# Read the comments and the script below to understand what's possible with Ren'Py and Raconteur!

# This is a comment, comments are only seen when looking at the script itself.
# This file contains the script for the Unity Raconteur demo. 
# Execution starts at the start label.

# Declare images used by this game.
image bg cafe = "cafe.png"
image bg office = "office.jpg"
image bg scenery = "scenery.jpg"

# Labels have blocks of text associated with them. Keep a look out for labels below.

define m = Character('Me', color='#f22')
define c = Character('Cashier', color='#f90')
define b = Character('Boss', color='#596')
define o = Character('Old Man', color='#349')

# The game starts here.
label start:

    # Start the background music playing.
    play music "theme.ogg" loop

    $ curious = False
    # This is a variable that will be used to remember player decisions later on.

    scene bg cafe
    # with fade
    # transitions are not supported yet

    m "It's another brisk day, but winter is fading."
    # Show old man
    m "I walk into this cafe every morning, and every morning, he's here."
    m "An older man with a leathery face. I wonder how old he actually is."
    m "He's dressed neatly, but his eyes seem tired. Always reading."
    m "I'm never here for long, just in for some coffee and out again."
    m "There's always so much to do..."

    scene bg cafe
    # with fade

    m "It's nicer out than it was yesterday."
    m "He's here again, shouldn't be surprised."
    m "The holidays are coming up soon. I wonder where his family is."

    # show cashier normal
    # with dissolve

    c "That'll be two bucks for the coffee sir."
    m "Oh, I should pay more attention."

    menu:
        "Here you go.":
            jump intro_go
        "Here's the money. Do you know who that old man is?":
            jump intro_who

label intro_go:
    scene bg cafe
    # with fade

    # show cashier normal
    # with dissolve
    c "Here's your coffe. You know that guy? You were staring at him pretty hard."
    m "Oh, no I don't. Thanks."
    jump intro_pass

label intro_who:

    # show cashier normal
    # with dissolve
    $ curious = True
    c "Not sure really to be honest. He's here every day from open to close with the same book."
    c "You could go ask him yourself, you know."
    m "Thanks."
    jump intro_pass

label intro_pass:
    scene bg cafe
    # with fade

    m "I wonder if I should go talk to him. Today isn't the best day though."
    m "I'm up for a promotion and I shouldn't be late for work."
    m "There he is, his eyes are motionless. I wonder..."

    menu:
        "Talk to the old man.":
            jump intro_talk
        "Leave cafe and go to work.":
            jump intro_work

label intro_talk:
    scene bg cafe
    #with fade

    if curious:
        "My curiosity got the better of me"
    else
        "I could have asked the cashier, but I'd rather be direct."

    m "Hello. I've seen you every day since I started my new job."
    m "Why do you spend so much time here?"
    # show oldman normal
    # with dissolve
    o "I like to listen."
    m "What do you mean? You're always reading that book."
    o "See the pages? They're blank."
    m "But why read a blank book?"
    o "I'm blind, and people are more comfortable with me staring at a book than nothing at all."
    o "Let me tell you my story."
    jump good_ending

label intro_work:
    scene bg cafe
    # with fade

    if curious:
        "Even though I was curious, I needed to get to work."
    else
        "I'm sure the cashier would have known who this man was. I needed to get to work."

    m "And so I went to work instead."
    scene bg office
    # with dissolve
    m "I got the promotion."
    m "I even got a stake in the company"
    # show boss
    # with dissolve
    b "Congratulations, you're going to do great."

label bad_ending:
    scene bg cafe
    # with fade
    m "Now I own the company, and I still visit that cafe."
    m "But I haven't seen the old man since that day."
    m "And every day, I wonder what would have happened if I had talked to him."
    ".:. Bad Ending."
    jump end

label good_ending:
    scene bg scenery
    # with fade
    m "So the Old Man, Roy, told me his story. He was a mountaineer, the first to scale many peaks."
    m "He never had a family growing up, and spent too much time on adventures for one of his own."
    m "Now he sits and remembers his life, while listening to the hustle and bustle inside the cafe."
    m "I've invited him to my house for a holiday dinner. Who knows what other stories he has to tell."
    ".:. Good Ending."
    jump end


label end:
    return