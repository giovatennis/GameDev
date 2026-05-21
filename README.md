Assignment 1:

Chose different falling speeds as the additional feature. The way I implemented this was by setting up a range of possible values using

float minSpeed = 2f;
float maxSpeed = 8f;

Then randomly selecting a value within that range by using

fallSpeed = Random.Range(minSpeed, maxSpeed);

inside of the Start() function for every new obstacle 
