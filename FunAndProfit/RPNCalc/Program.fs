// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

(* stack data type*)
// type Stack = float list
type Stack = StackContents of float list

let newStack = StackContents [1.0; 2.0; 3.0]

let (StackContents contents) = newStack

(*the push function*)
// let push x aStack =
//     let (StackContents contents) = aStack
//     let newContents = x::contents
//     StackContents newContents
let push x (StackContents contents) =
    StackContents (x::contents)

// let emptyStack = StackContents []
// let stackWith1 = push 1.0 emptyStack
// let stackWith2 = push 2.0 stackWith1

// let ONE stack = push 1.0 stack
// let TWO stack = push 2.0 stack
let ONE = push 1.0
let TWO = push 2.0
let THREE = push 3.0
let FOUR = push 4.0
let FIVE = push 5.0
let EMPTY = StackContents []

let stackWith1 = ONE EMPTY
let stackWith2 = TWO stackWith1
let stackWith3 = THREE stackWith2

let result123 = EMPTY |> ONE |> TWO |> THREE
let result312 = EMPTY |> THREE |> ONE |> TWO

(*popping the stack*)
/// pop a value from the stack and return it
/// *and* the new stack as a tuple
let pop (StackContents contents) =
    match contents with
    | top::rest ->
        let newStack = StackContents rest
        (top,newStack)
    | [] ->
        failwith "Stack underflow"

let initialStack = EMPTY  |> ONE |> TWO
let popped1, poppedStack = pop initialStack
let popped2, poppedStack2 = pop poppedStack

// let _ = pop EMPTY  // throws underflow exception

(*arithmetic funcitons*)
// let ADD stack = 
//     let x,s = pop stack
//     let y,s2 = pop s
//     let result = x + y
//     push result s2
// let MUL stack = 
//     let x,s = pop stack
//     let y,s2 = pop s
//     let result = x * y
//     push result s2
let binary mathFn stack =
    let y,stack' = pop stack
    let x,stack'' = pop stack'
    let z = mathFn x y
    push z stack''
//let ADD aStack = binary (fun x y -> x + y) aStack
let ADD = binary (+)
let SUB = binary (-)
let MUL = binary (*)
let DIV = binary (/)

let add1and2 = EMPTY |> ONE |> TWO |> ADD
let add2and3 = EMPTY |> TWO |> THREE |> ADD
let mult2and3 = EMPTY |> TWO |> THREE |> MUL

let unary f stack =
    let x,stack' = pop stack
    push (f x) stack'

let NEG = unary (fun x -> -x)
// let SQUARE = unary (fun x -> x * x)

let SHOW stack =
    let x,_ = pop stack
    printfn "The answer is %f" x
    stack

let DUP stack =
    let x,_ = pop stack
    push x stack
let SWAP stack =
    let x,s = pop stack
    let y,s' = pop s
    push y (push x s')
let START = EMPTY

let SQUARE =
    DUP >> MUL

let CUBE = 
    DUP >> DUP >> MUL >> MUL

let SUM_NUMBERS_UPTO =
    DUP
    >> ONE
    >> ADD
    >> MUL
    >> TWO
    >> DIV
