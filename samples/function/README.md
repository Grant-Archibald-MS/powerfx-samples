# Overview

This sample demonstrates the ability apply a namespace to a function.  In this example, we have a function called "Common.Start" in a namespace called "Common". If you don't add the namespace to the function, and you later decide to add a function called "Start" to the root namespace, there could be a naming conflict.

This is because both functions would have the same name, "Start", and the app wouldn't know which one to use. By adding the namespace to the "Common.Start" function, you are future-proofing your app and ensuring that there won't be any naming conflicts if a "Start" function is added to the root namespace in the future.

So, adding a namespace to a Power FX function is important not only to prevent current naming conflicts, but also to future-proof your app and prevent potential conflicts down the line.
