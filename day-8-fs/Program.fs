type OpCode = Nop | Acc | Jmp
type Instruction = Instruction of OpCode * int
type ProgramState = {Acc: int; Position: int}
type ProgramResult = TerminatedNormally | HitInifiniteLoop

let opcode = function | "nop" -> Nop | "acc" -> Acc | "jmp" -> Jmp
let opcodeOf (Instruction(op, _)) = op

let instruction (inst:string) = Instruction (opcode inst.[0..2], int inst.[4..])

let executeInstruction state (inst:Instruction) = 
    match inst with  
        | Instruction (Nop, _) -> { state with Position = state.Position + 1}
        | Instruction (Jmp, offset) -> { state with Position = state.Position + offset}
        | Instruction (Acc, value) -> { state with Position = state.Position + 1; Acc = state.Acc + value}

type FnPatch = Instruction -> Instruction option

let rec run (prog:Instruction array) (patcher:FnPatch option) (visited:Set<int>) (state:ProgramState)  =    
    match state.Position with
        | position when position = prog.Length -> TerminatedNormally, state
        | position when visited.Contains position -> HitInifiniteLoop, state        
        | position -> let runNext instruction = run prog patcher (visited.Add position) (executeInstruction state instruction)  
                      let instruction = prog.[position]                      
                      match patcher with 
                      | Some patch -> match patch instruction with
                                        | Some alt -> match runNext alt with
                                                        | (TerminatedNormally, state) -> (TerminatedNormally, state)
                                                        | (_, _) -> runNext instruction
                                        | None -> runNext instruction
                      | None -> runNext instruction

let runProgram prog (patcher:FnPatch option) =
    let initialState = {Position = 0; Acc = 0}
    let visited = set []  
    run prog patcher visited initialState    

let program = System.IO.File.ReadLines("input.txt") |> Seq.map instruction |> Seq.toArray

let p1Result, p1State = runProgram program None
printfn "Part 1: (%A) Final Accumulator = %i" p1Result p1State.Acc
                   
let patcher = function | Instruction(Jmp, arg) -> Some(Instruction(Nop, arg))
                       | Instruction(Nop, arg) -> Some(Instruction(Jmp, arg))
                       | other -> None

let p2Result, p2State = runProgram program (Some patcher)
printfn "Part 2: (%A) Final Accumulator = %i" p2Result p2State.Acc 
