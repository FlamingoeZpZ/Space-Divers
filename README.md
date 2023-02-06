# Space-Divers

(NOTE: I added the images after submission because they're not necessary, but they are convinent)


##Lighting
Wasn't sure what to do for the "light section" Unsure if you wanted me to actually edit the light in unity or to make shaders... I asked others and wasn't sure, and I really don't want to be the guy that's consistantly asking for help. so hopefully this good.

Ambient + Rim Light
Ambient + Diffuse (Lambert Model) + Rim Light
Ambient + Diffuse + Specular (Blinn Model) + Rim Light
Specular + Diffuse + Rim Light 

The Ambient Diffuse textures I took help from the official unity form (See below) it's very similar with minor property changes to give more control
The Blinn shader is taken from class, again minor changes for increased control

Why rim light? Because it counts for one of the sections, and looks like atmosphere

##LUTS

Color Grading: 

Normal:

Cool:

Warm:

Custom (Warm + Contrast): 


LUTs seem to always look bad, not sure why... They look really good in Photoshop

##Custom
Rim lighting (Explained earlier)
Normal extrusion ( Bump mapping) Gives the game a more "BorderLandsy" Rough and tumble type look. Also helps improve areas that got color graded too hard when I was making the texture white. When normals are 1, everything looks like a toy, when normals are 10 everything looks borderlands





## NOTES 

There is a lot of cool things I didn't get to show off

I'm taking advantage of all OOP principles (Encapsulation, Polymorphism, Inheritance, Abstraction), 

Statics for the WIP blip system, 

I have multiple design patterns (Flyweight, Singletons (light use tho))

The code for the color picker, though I'm sure you know how it works, it's super simple and the manipulation of it in game would be almost identical to how the LUTs are handled.

##Resources and Links

Youtube Video
https://youtu.be/mPkiP0KxTGw

Google slides
[https://docs.google.com/presentation/d/1b1NrIQdTX083GBhReAvFBrGcwF4PFgROCB6B2utCwPA/edit?usp=sharing]

Unity shader fundamentals: (I needed it to help figure out how to combine the code because the shaders from class were written with vertex and frag instead of surface shaders)

https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html

Star sparrow modular space ship
https://assetstore.unity.com/packages/3d/vehicles/space/star-sparrow-modular-spaceship-73167

Joystick Pack (Because I'm lasy, and why remake the wheel)
https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631

Translucent Crystals (In project, not used)
https://assetstore.unity.com/packages/3d/environments/fantasy/translucent-crystals-106274


