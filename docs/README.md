[Advent of Code](https://adventofcode.com/) competition, year 2024.

Work in progress.

# Helpful VS Code keybinding

Now I can press **[Ctrl]+[K],[I]** to toggle inline "lens"
hints on and off!

The **Ionide for F#** extension for Visual Studio Code
has some "lens" features enabled by default that decorate
my code with inline parameter and data type hints. I have found them
very helpful while I learn and practice F#.
But sometimes the decorations get in the way, or obscure
what my F# code actually *is*, because it has inserted extra hints about
what my code *means*.

So for convenience while I'm learning, a genAI chatter helped me configure
this useful VS Code key binding configuration -- below is my
`keybindings.json` file. I created it using Command Palette's
**"Preferences: Open Keyboard Shortcuts (JSON)"** configuration.
On my computer it's in my `C:\Users\<username>\AppData\Roaming\Code\User\`
folder because that's where VS Code put it. I chose [Ctrl]+[K],[I]
because that key-command sequence was unused and because it seemed
a good mnemonic for both *inline* and *Ionide.* It depends on another
VS Code extension, Cody Hoover's **Settings Cycler**.

```json
[

  {
    "key": "ctrl+k i",
    "command": "settings.cycle",
    "args": {
      "id": "fsharpInlayHintsAndLineLens",
      "overrideWorkspaceSettings": true,
      "values": [
        {
          "[fsharp]": { "editor.inlayHints.enabled": "off" },
          "FSharp.lineLens.enabled": "never",
          "FSharp.codeLenses.signature.enabled": false
        },
        {
          "[fsharp]": { "editor.inlayHints.enabled": "on" },
          "FSharp.lineLens.enabled": "always",
          "FSharp.codeLenses.signature.enabled": true
        }
      ]
    },
    "when": "editorTextFocus && editorLangId == 'fsharp'"
  }

]
```
