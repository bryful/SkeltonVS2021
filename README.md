# Skelton

C#アプリを作成する時にベースとなるプロジェクトファイル  
  
プロジェクトテンプレートにして使います。  
  
.Net Core対応とSystem.Text.Jsonに対応の為にupdateしました（まだ途中)  
Jsonの扱いをSynamicJsonからSystem.Text.Jsonにしました。
  
.NetframeworkはILMergeを使ってdllをexeにまとめています。
.Net coreは発行でまとめる様にしています。  
  
しかし.Net coreは容量がでかい！  
  
PrefFile.csは設定ファイルを保存するためのclassです。System.Text.Jsonに書き換えました。  
  
AppInfoDialog.csはAboutDialogです。  
以前はアセンブリから情報を獲得していましたが、ILMerge使うとおかしくなるので、プロパティに設定する方法に変えました。

# License

This software is released under the MIT License, see LICENSE. 

# Authors

bry-ful(Hiroshi Furuhashi)  
twitter:[bryful](https://twitter.com/bryful)  
bryful@gmail.com  

# References

ファイル名・ファイルパスから 特大アイコン (48x48 256x256 ピクセル) を取得する (C#プログラミング)  
https://www.ipentec.com/document/csharp-shell-namespace-get-big-icon-from-file-path  


