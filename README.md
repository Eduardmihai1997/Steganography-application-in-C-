<h1>Steganography application</h1>


<h2>Description</h2>
A WPF application that enables users to hide a message within an image using steganography.Users can select an image, input a message, and then encode the message into the image. The modified image can be saved, preserving the hidden message. The application also allows users to decode the message from the encoded image. The program follows the MVVM architectural pattern, using the MVVM Community Toolkit for binding commands and properties between the ViewModel and View. The steganography service provided within the application handles the encoding and decoding processes for embedding and extracting messages. It utilizes dependency injection to manage dependencies and improve maintainability and testability.

<h2>Languages Used</h2>

- <b>C#</b> 
- <b>WPF</p>


<h2>Environments Used </h2>

- <b>Microsoft Viseual Studio Code</b> (21H2)

<h2>Program walk-through:</h2> 

- <b>Program interface:</b>

<img src="https://i.imgur.com/Iiqa8Qd.png" height="80%" width="80%"/>
<br></br>

- <b>Loading the image with the "Loading" button</b>  
<img src="https://i.imgur.com/Ljogdcl.png" height="80%" width="80%" />
<br></br>


- <b>Write a secret message in the Textbox then click "Encode" to encode the image</b> > 
 <img src="https://i.imgur.com/KHxzahT.png" height="80%" width="80%" />
 <br></br>

- <b>Now you can save the ecoded image with the "Save encoded image" button</b> 
- <b>You can use de "Decode" button to decode secrete messages from images</b>
<img src="https://i.imgur.com/UROPsuz.png" height="80%" width="80%" />
 <br></br>
