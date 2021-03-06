﻿[<AutoOpen>]
/// Contains helper functions which allow to get and set the git commit message.
module Fake.Git.CommitMessage

open Fake
open System
open System.Text
open System.IO


/// Returns the commit message file.
let getCommitMessageFileInfos repositoryDir =
    let gitDir = findGitDir repositoryDir
    [gitDir.FullName </> "COMMITMESSAGE" |> fileInfo
     gitDir.FullName </> "COMMIT_EDITMSG" |> fileInfo ]

/// Gets the commit message
let getCommitMessage repositoryDir = 
    match getCommitMessageFileInfos repositoryDir |> List.filter (fun fi -> fi.Exists) with
    | fi::_ -> ReadFileAsString fi.FullName 
    | _ -> ""       

/// Sets the commit message
let setMessage repositoryDir text =
    for messageFile in getCommitMessageFileInfos repositoryDir do
        if isNullOrEmpty text then
            if messageFile.Exists then messageFile.Delete()
        else
            use textWriter = new StreamWriter(messageFile.FullName, false, new UTF8Encoding(true))
            textWriter.Write text