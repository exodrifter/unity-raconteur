
# This is the working script for the Unity package, Raconteur, designd to integrate Ren'Py directly and seamlessly into Unity. 
# Read the comments and the script below to understand what's possible with Ren'Py and Raconteur!

# This is a comment, comments are only seen when looking at the script itself.
# This file contains the script for the Unity Raconteur demo. 
# Execution starts at the start label.

# Labels have blocks of text associated with them. Keep a look out for labels below.

define r = Character('R', color='#f22')

# The game starts here.
#begin start
label start:

    #end start

    # Start the background music playing.
    #play music "theme.ogg"

    $ name_arnold = False
    $ name_felicia = False
    $ name_q = False
    $ name = "kid"
    $ just_two = False
    $ olivia = False
    $ paulie = False
    $ sal = False
    $ tempest = False

    #window show

    r "Oof! Huh?"
    r "Damn it!"
    r "Nevermind, it's not safe now. This operation is a bust. We've got to head out of here."
    # play shuffling sound
    r "Okay, it looks like the coast is clear."
    r "I was about to make a very clean swipe of the most famous painting in the world before you messed up my timing."
    r "We'll have to try again. You owe me now, and people always pay me back."
    r "Walk and talk, kid. Let's keep moving."
    # play ambient city

    menu:
        "Who are you?":
            jump intro_who
        "Where are we?":
            jump intro_where
        "What's going on?":
            jump intro_whatA

label intro_where:

    r "Are you kidding me? This is New York! You need to eat your wheaties."
    r "The building where we met is a little place called the Museum of Modern Art. Not exactly an easy place to forget."
    r "I was having a real nice visit until you couldn't watch where you were going."
    r "Yeah, New York ain't without its problems. A few bad pizzerias. One too many up and comers. There's the mob, gangs, and even children."
    r "Children terrify me."
    r "But if you can get past the kids, it's the perfect place for a professional such as myself."
    r "Great food, lots of wealthy individuals, and, of course, art!"

    menu:
        "So you're a criminal?":
            jump intro_who2
        "How did I mess up your operation?":
            jump intro_whatA

label intro_where2:

    r "Are you kidding me? This is New York! You need to eat your wheaties."
    r "The building where we met is a little place called the Museum of Modern Art. Not exactly an easy place to forget."
    r "I was having a real nice visit until you couldn't watch where you were going."
    r "Yeah, New York ain't without its problems. A few bad pizzerias. One too many up and comers. There's the mob, gangs, and even children."
    r "Children terrify me."
    r "But if you can get past the kids, it's the perfect place for a professional such as myself."
    r "Great food, lots of wealthy individuals, and, of course, art!"

    menu:
        "So are you some sort of thief?":
            jump intro_where2
        "How did I mess up your operation?":
            jump intro_whatD

label intro_who:

    r "Who am I?"
    # play shiny sound
    r "Just call me R. I'm highly skilled in what you might call the delicate art of theft."
    r "You may be asking why I'm so open about my skills as a moonlighting artisan."
    r "I know how to read people, and your eyes tell me everything I need to know about you. You're not a cop."
    r "Do you hear that back there? Pigs are all over the museum at this point."
    r "Things were clockwork until you got in my way."

    menu:
        "So you're a criminal?":
            jump intro_who2
        "What happened when I got in the way?":
            jump intro_whatB

label intro_who2:

    r "If you play by the paltry rules of society, then sure, someone like me is a criminal."
    r "I don't play those rules. I'm an artist. And my art is stealing."
    r "But I don't steal just anything. No, I steal art."
    r "Fine paintings and antiquities of yore. Bagging these relics are what I live for."
    r "Of course, they also fetch a nice price with the right buyer. But money isn't my goal."
    r "When I've collected enough pieces, I'll open my own exclusory gallery."
    r "The greatest collection of art ever assembled. Only the ritziest upper crust will know about it."
    r "Even the Louvre will feel like an elementary school art show in comparison."
    r "I'm gonna provide the ultimate, but before that we've got to make this heist happen."
    r "The heist that was dialed in before you came out of nowhere."

    menu:
        "What is the heist?":
            jump intro_whatB
        "How did I manage to mess up your operation?":
            jump intro_whatC

label intro_whatA:

    r "You certainly cut to the quick, kid."
    r "I like that."
    r "You happened to interrupt me during a 5 second window to get past security at the art museum and blew my heist."
    r "So now you're going to help me make this heist happen."
    r "It's the heist to end all heists."
    r "My dream is to create the perfect art gallery. The ultimate experience, only for the ritziest, most secretive upper crust."
    r "And I'm looking for my crown jewel. A particular painting."
    r "The painting is a work from of a buddy of mine, Vinny van Gogh. Starry Night. Heard of it?"
    r "Thought so. Well, I just lost my chance for a solo operation. I'll need to get a team together."
    r "I could use a bright face like you as the decoy. Whaddya say? You in? You don't have much of a choice, by the way."

    menu:
        "Let's do it!":
            jump intro_whyA
        "What's in it for me?":
            jump intro_whyB
        "I'm not a thief.":
            jump intro_whyC

label intro_whatB:

    r "You certainly cut to the quick, kid."
    r "I like that."
    r "This is a heist to end all heists."
    r "I've got a business associate who will fund the rest of the logistics for the gallery if I can pull this off."
    r "The painting is a work from of a buddy of mine, Vinny van Gogh. Starry Night. Heard of it?"
    r "Thought so. Well, I just lost my chance for a solo operation. I'll need to get a team together."
    r "I could use a bright face like you as the decoy. Whaddya say? You in?"

    menu:
        "Let's do it!":
            jump intro_whyA
        "What's in it for me?":
            jump intro_whyB
        "I'm not a thief.":
            jump intro_whyC

label intro_whatC:

    r "I've got an inside man. Vincenzo."
    r "I had a 5 second window to get into the vault where the painting was being conditioned."
    r "I was waltzing in, then you got in my way. I couldn't run past the guard station without gathering some attention at that point."
    r "But it's fine. I didn't exactly have an escape plan figured out. And now you're gonna help me see this through."
    r "This is a heist to end all heists."
    r "I've got a business associate who will fund the rest of the logistics for the gallery if I can pull this off."
    r "The painting is a work from of a buddy of mine, Vinny van Gogh. Starry Night. Heard of it?"
    r "Thought so. Well, I just lost my chance for a solo operation. I'll need to get a team together."
    r "I could use a bright face like you as the decoy. Whaddya say? You in?"

    menu:
        "Let's do it!":
            jump intro_whyA
        "What's in it for me?":
            jump intro_whyB
        "I'm not a thief.":
            jump intro_whyC

label intro_whyA:

    r "Your enthusiasm gives me hope for the future. Truly. You got a name?"

    menu:
        "I'm Arnold.":
            $ name_arnold = True
            $ name = "Arnie"
            jump intro_finish
        "Felicia is the name.":
            $ name_felicia = True
            $ name = "Dolly"
        "Just call me Q.":
            $ name_q = True
            $ name = "Q"
            jump intro_finish


label intro_whyB:

    r "How does a chance to be part of something larger than life sound? Otherwise I can arrange some kidney damage for you if that sounds more appealing."
    r "What's your handle, kid?"

    menu:
        "I'm Arnold.":
            $ name_arnold = True
            $ name = "Arnie"
            jump intro_finish
        "Felicia is the name.":
            $ name_felicia = True
            $ name = "Dolly"
        "Just call me Q.":
            $ name_q = True
            $ name = "Q"
            jump intro_finish


label intro_whyC:

    r "You're either going to be a thief or a dead goody two-shoes. Think about it? Yeah? That was a quick decision. What's your name?"

    menu:
        "I'm Arnold.":
            $ name_arnold = True
            $ name = "Arnie"
            jump intro_finish
        "Felicia is the name.":
            $ name_felicia = True
            $ name = "Dolly"
        "Just call me Q.":
            $ name_q = True
            $ name = "Q"
            jump intro_finish

label intro_finish:

    if name_arnold == True:
        r "Arnold? I don't like the sound of that. Let's try, Arnie."
    if name_felicia == True:
        r "That's fiesty. Felicia. But we can't have you running around with a name like that. We'll call you Flo."
    if name_q == True:
        r "I see what you're getting at. And I like it. You've got it, Q."
    r "But we've got to get this show on the road, %(name)s."
    r "The reason I had this project in the bag was because I had a guy on the inside."
    r "Vincenzo. He's a good guy, but he has a couple of loose screws if you know what I mean."
    r "Since our cover got blown, he's gonna take some heat for this. But he won't rat."
    r "There's nothing I hate more than a rat."
    r "Anyway, it's the two of us, and Vincenzo is sitting this one out. He might be sitting this one out in the big house."
    r "We need a couple more hands on deck to make this happen."
    r "I've got 4 people in mind, but we really only need two of 'em. How's about you help me figure out who we're going to take?"

    menu:
        "I think the two of us can handle it.":
            $ just_two = True
            jump intro_finish2
        "Who do you know?":
            jump intro_finish2


label intro_finish2:

    if just_two == True:
        r "You've got spunk. But this is bigger than the two of us."
    r "These are four individuals that I've had the pleasure of working with in the past to great success."
    r "But they ain't perfect. And not all of 'em get along, if you know what I mean. Here's the breakdown."
    r "First, we've got Olivia. Smoothest operator in the business. She can get passwords, keys, you name it. Never can tell what her end game is."
    r "Next up is Paulie. He's hotheaded and only after cash. His musclehead look may suggest otherwise, but damn if he can't outsmart a security system."
    r "We can't have Olivia and Paulie together: they do the same job. That's bad economics 101. Take notes, %(name)s."
    r "Not to mention, we've got to pay off all our operatives since the painting is going in the gallery. The less mouths to feed the better."
    r "Then there's Sal. Cold. That's what you feel, just looking at him. Eye contact makes me shiver. He's a getaway driver with nerves of steel."
    r "Last and certainly not least, we've got Tempest. She's a hurricane that takes the form of a dame. Drives a boosted import."
    r "When it comes to drivers, it's really a matter of style. Let's start with that. Do you want to leave a trail of flames or zip out like nothing happened?"

    menu:
        "Let's go with a cool and collected getaway - Sal.":
            $ sal = True
            jump intro_finish3
        "Let's go out in style - Tempest.":
            $ tempest = True
            jump intro_finish3

label intro_finish3:

    if sal == True:
        r "Sal also happens to be about 70 years old. He was driving when they still used horses."
    if tempest == True:
        r "All right. I hope you know how to put a seat belt on."
    r "We've got one more pick, and this is the big one. Who's going to be our shaker?"
    r "Olivia and Paulie come with their own challenges."
    r "Olivia is a master at getting information. But her smoothness scares me sometimes. She could outcon me someday."
    r "Paulie has a brain as big as his biceps. His temper and tunnel vision for cash have led to some close calls."
    r "What does your heart say about this one, %(name)s?"

    menu:
        "We need the subtlety - Olivia.":
            $ olivia = True
            jump intro_end
        "Can't go wrong with brain and brawn - Paulie.":
            $ paulie = True
            jump intro_end

label intro_end:

    if olivia == True:
        r "She's a genius at getting information and getting people to what she wants - for better or worse."
    if paulie == True:
        r "If it comes down to it, I don't think I could take the guy in a scrap. We'll have to keep him calm."
    r "So we've got a team."
    r "The next step is to get us all in the same place. There's no going back after this, kid."

    return

