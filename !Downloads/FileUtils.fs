// FileUtils.fs
namespace Utilities

open System
open System.IO
open System.Text
open System.Threading.Tasks

module FileUtils =
    /// Canonical: read entire file as a string using System.IO.File.ReadAllText
    let readAllText (path: string) : string =
        File.ReadAllText(path)

    /// Canonical with explicit encoding (UTF-8 by default)
    let readAllTextWithEncoding (path: string) (encoding: Encoding) : string =
        File.ReadAllText(path, encoding)

    /// Safer pattern: return Result<string, exn> instead of throwing
    let tryReadAllTextResult (path: string) : Result<string, exn> =
        try
            Ok (File.ReadAllText(path))
        with ex -> Error ex

    /// Option-based variant: Some text or None
    let tryReadAllTextOption (path: string) : string option =
        try
            Some (File.ReadAllText(path))
        with _ -> None

    /// Async (Task) variant using ReadAllTextAsync (requires .NET that supports it)
    let readAllTextAsync (path: string) : Task<string> =
        File.ReadAllTextAsync(path)

    /// StreamReader variant: useful for large files when you want to process incrementally
    let readLinesSeq (path: string) : seq<string> =
        seq {
            use sr = new StreamReader(File.OpenRead(path))
            while not sr.EndOfStream do
                yield sr.ReadLine()
        }

    /// Read as bytes then decode (gives control over binary vs text handling)
    let readAllTextFromBytes (path: string) (encoding: Encoding) : string =
        let bytes = File.ReadAllBytes(path)
        encoding.GetString(bytes)
