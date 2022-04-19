# BUREAUcracy

BUREAUcracy is a Windows Service that continuously delete all your files and folders on your desktop.

## ✉ Import the project

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### 🖥 Prerequisites

- Windows 10
- Visual Studio 2017 / 2019 / 2022

### ⛏ Import

To get started, download the project on GitHub. Unzip the folder somewhere on your computer. Then open the "BUREAUcracy.sln" file with Visual Studio. There you go. The project is open and you can edit it at your convenience.

## 💿 Deployment

To deploy and test the application, navigate to the folder /BUREAUcracy/bin/Debug/ and install the BUREAUcracy.exe service using this tutorial : https://docs.microsoft.com/en-us/dotnet/framework/windows-services/how-to-install-and-uninstall-services#install-using-installutilexe-utility.

## If you do not want to read the boring Microsoft tutorial

1. Open cmd
2. Navigate to C:\Windows\Microsoft.NET\Framework\v4.0.30319
3. Write InstallUtil.exe + The location of BUREAUcracy.exe
4. Write net start BUREAUcracy
5. Enjoy

## ✒ Authors

* **Bruno Paiva** - https://vergasta.ch

## 📃 License

This project is licensed under the GNU General Public License Version 3 - see the [LICENSE.md](LICENSE.md) file for details.

## 📥 Download

Download this project : https://github.com/brunopaiva15/BUREAUcracy
