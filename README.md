# McCulloch-Pitts Neuron implementation in Unity

This is something I did for fun in a couple of hours: a simulation system for McCulloch-Pitts Neural Networks.

Mostly inspired by this article: https://towardsdatascience.com/mcculloch-pitts-model-5fdf65ac5dd1.

You can create (by dragging from the prefabs) 3 types of components:
* A generator - can generate a false or a true
* An output - Is lit (green) when the input is true
* A neuron - Following the McCulloch-Pitts model, it has dendrites (connections to others) and axons (not sure if the original implementation only has one axon like a real neuron, but for this one we don't even keep track of them). It also has a theta value (a threshould).

The default scene has the implementation of a network that lights the lamp everytime it sees two "true" values on the inputs. The true values can be separated by any false values, but three true values will not leave the lamp lit (it resets after the second true value).

There is a propagation time inherent to the system itself, so if you want to do the sequence 1001, you have to play the scene on Unity, then press the 1 button, then "Run Step" to play the 1 itself, then set the generator to 0, press the "Run Step" button and so forth. The light should only light one step after you press the last "1" in the sequence and after you press the "Run Step" button, on this case...

If you build something interesting with this, let me know!

![Image](https://github.com/DiogoDeAndrade/McChullochPittsNeurons/raw/master/Screenshots/screen01.png)

## Credits

* Code, some art, game design done by [Diogo Andrade]

## Licenses

All code in this repo is made available through the [GPLv3] license.
The text and all the other files made by [Diogo Andrade] are made available through the [CC BY-NC-SA 4.0] license.

## Metadata

* Autor: [Diogo Andrade][]

[Diogo Andrade]:https://github.com/DiogoDeAndrade
[GPLv3]:https://www.gnu.org/licenses/gpl-3.0.en.html
[CC-BY-SA 3.0.]:http://creativecommons.org/licenses/by-sa/3.0/
[CC BY-NC-SA 4.0]:https://creativecommons.org/licenses/by-nc-sa/4.0/
