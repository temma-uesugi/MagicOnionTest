#!/bin/bash

mpc -I Shared/Shared.csproj -o MagicOnionTesttUnity/Assets/MagicOnionGen/MessagePackGen.cs
dotnet moc -I Shared/Shared.csproj -o MagicOnionTesttUnity/Assets/MagicOnionGen/MagicOnionGen.cs

echo finished 