Console.WriteLine("Welcome to the machine!");
ushort aRegister = 0;
ushort bRegister = 0;
ushort cRegister = 0;
ushort xRegister = 0;
ushort yRegister = 0;
ushort[] stack = new ushort[65536];
ushort[] codeRam = new ushort[65536];
ushort[] workRam = new ushort[65536];
bool flag = false;

ushort instructionInput()
{
    Console.WriteLine("Enter instruction!");
    string inputString = Console.ReadLine();
    string[] inputArray = inputString.Split(' ');
    bool isValidInstruction = false;
    while (!isValidInstruction)
    {
        isValidInstruction = true;
        if (inputString != "")
        {
            if (inputArray.Length != 3)
            {
                Console.WriteLine("Invalid instruction! Try again!");
                inputString = Console.ReadLine();
                inputArray = inputString.Split(' ');
                continue;
            }
            if (!Instructions.Operators.ContainsKey(inputArray[0]))
            {
                Console.WriteLine("Invalid instruction! Try again!");
                inputString = Console.ReadLine();
                inputArray = inputString.Split(' ');
                continue;
            }
            if (!Instructions.SubOperators.ContainsKey(inputArray[1]))
            {
                Console.WriteLine("Invalid instruction! Try again!");
                continue;
            }
            if (!int.TryParse(inputArray[2], out int operatorValue))
            {
                Console.WriteLine("Invalid instruction! Try again!");
                inputString = Console.ReadLine();
                inputArray = inputString.Split(' ');
                continue;
            }
        }
    }
    ushort instructionInt = 0;
    if (inputString != "")
    {
        instructionInt += (ushort)(Instructions.Operators[inputArray[0]] * 64);
        instructionInt += (ushort)(Instructions.SubOperators[inputArray[1]] * 16);
        instructionInt += ushort.Parse(inputArray[2]);
    }
    return instructionInt;
}
string instructionToString(ushort instruction)
{
    byte operatorValue = (byte)(instruction / 64);
    byte suboperatorValue = (byte)((instruction % 64) / 16);
    byte operandValue = (byte)(instruction % 16);
    string operatorString = Instructions.ReverseOperators[operatorValue];
    string suboperatorString = Instructions.ReverseSubOperators[suboperatorValue];
    string operatorHexString = operatorValue.ToString("X2");
    return $"{operatorString} {suboperatorString} {operandValue} - " +
        $"0x{operatorValue.ToString("X2")} 0x{suboperatorValue.ToString("X2")} 0x{operandValue.ToString("X2")}";
}
void programPrint()
{
    for (ushort i = 0; i < codeRam.Length; i++)
    {
        if (codeRam[i] != 0)
        {
            Console.WriteLine($"{i + 1}:{instructionToString(codeRam[i])}");
        }
    }
}
void programClear()
{
    for (ushort i = 0; i < codeRam.Length; i++)
    {
        codeRam[i] = 0;
    }
}
void stackPush(ushort value)
{
    for (ushort i = 0; i < stack.Length; i++)
    {
        if (stack[i] == 0)
        {
            stack[i] = value;
            break;
        }
    }
}
ushort stackPop()
{
    ushort value = 0;
    for (ushort i = 0; i < stack.Length; i++)
    {
        if (stack[i] == 0)
        {
            value = stack[i - 1];
            stack[i - 1] = 0;
            break;
        }
    }
    return value;
}
ushort instructionExecute(ushort instruction)
{
    byte operatorValue = (byte)(instruction / 64);
    byte suboperatorValue = (byte)((instruction % 64) / 16);
    byte operandValue = (byte)(instruction % 16);
    string operatorString = Instructions.ReverseOperators[operatorValue];
    string suboperatorString = Instructions.ReverseSubOperators[suboperatorValue];
    ushort jumpTo = 0;
    switch (operatorString)
    {
        case "PRC":
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine("");
                    break;
                case "MAX":
                    Console.WriteLine($"A register value: {aRegister}");
                    Console.WriteLine($"B register value: {bRegister}");
                    Console.WriteLine($"C register value: {cRegister}");
                    Console.WriteLine($"X register value: {xRegister}");
                    Console.WriteLine($"Y register value: {yRegister}");
                    break;
                case "ARB":
                    Console.WriteLine($"Arbitrary value: {operandValue}");
                    break;
                case "ARG":
                    Console.WriteLine($"A register value: {aRegister}");
                    break;
                case "BRG":
                    Console.WriteLine($"B register value: {bRegister}");
                    break;
                case "ACC":
                    Console.WriteLine($"C register value: {cRegister}");
                    break;
                case "XRG":
                    Console.WriteLine($"X register value: {xRegister}");
                    break;
                case "YRG":
                    Console.WriteLine($"Y register value: {yRegister}");
                    break;
                case "STK":
                    Console.WriteLine($"Stack value: {stackPop()}");
                    break;
                case "CRM":
                    Console.WriteLine($"Code ram value {operandValue}: {codeRam[operandValue]}");
                    break;
                case "WRM":
                    Console.WriteLine($"Work ram value {operandValue}: {workRam[operandValue]}");
                    break;
            }
            break;
        case "PBC":
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine("00000000");
                    break;
                case "MAX":
                    Console.WriteLine("11111111");
                    break;
                case "ARB":
                    Console.WriteLine(Convert.ToString(operandValue, 2));
                    break;
                case "ARG":
                    Console.WriteLine(Convert.ToString(aRegister, 2));
                    break;
                case "BRG":
                    Console.WriteLine(Convert.ToString(bRegister, 2));
                    break;
                case "ACC":
                    Console.WriteLine(Convert.ToString(cRegister, 2));
                    break;
                case "XRG":
                    Console.WriteLine(Convert.ToString(xRegister, 2));
                    break;
                case "YRG":
                    Console.WriteLine(Convert.ToString(yRegister, 2));
                    break;
                case "STK":
                    Console.WriteLine(Convert.ToString(stackPop(), 2));
                    break;
                case "CRM":
                    Console.WriteLine(Convert.ToString(codeRam[operandValue], 2));
                    break;
                case "WRM":
                    Console.WriteLine(Convert.ToString(workRam[operandValue], 2));
                    break;
            }
            break;
        case "PHC":
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine("0x00");
                    break;
                case "MAX":
                    Console.WriteLine("0xFF");
                    break;
                case "ARB":
                    Console.WriteLine($"{operandValue.ToString("X")}");
                    break;
                case "ARG":
                    Console.WriteLine($"{aRegister.ToString("X")}");
                    break;
                case "BRG":
                    Console.WriteLine($"{bRegister.ToString("X")}");
                    break;
                case "ACC":
                    Console.WriteLine($"{cRegister.ToString("X")}");
                    break;
                case "XRG":
                    Console.WriteLine($"{xRegister.ToString("X")}");
                    break;
                case "YRG":
                    Console.WriteLine($"{yRegister.ToString("X")}");
                    break;
                case "STK":
                    Console.WriteLine($"{stackPop().ToString("X")}");
                    break;
                case "CRM":
                    Console.WriteLine($"{codeRam[operandValue].ToString("X")}");
                    break;
                case "WRM":
                    Console.WriteLine($"{workRam[operandValue].ToString("X")}");
                    break;
            }
            break;
        case "PDC":
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine(0);
                    break;
                case "MAX":
                    Console.WriteLine(65535);
                    break;
                case "ARB":
                    Console.WriteLine(operandValue);
                    break;
                case "ARG":
                    Console.WriteLine(aRegister);
                    break;
                case "BRG":
                    Console.WriteLine(bRegister);
                    break;
                case "ACC":
                    Console.WriteLine(cRegister);
                    break;
                case "XRG":
                    Console.WriteLine(xRegister);
                    break;
                case "YRG":
                    Console.WriteLine(yRegister);
                    break;
                case "STK":
                    Console.WriteLine(stackPop());
                    break;
                case "CRM":
                    Console.WriteLine(codeRam[operandValue]);
                    break;
                case "WRM":
                    Console.WriteLine(workRam[operandValue]);
                    break;
            }
            break;
        case "PSC":
            short signedValue;
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine(-32768);
                    break;
                case "MAX":
                    Console.WriteLine(32767);
                    break;
                case "ARB":
                    if (operandValue < 32768)
                    {
                        signedValue = (short)operandValue;
                    }
                    else
                    {
                        signedValue = (short)(operandValue - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "ARG":
                    if (aRegister < 32768)
                    {
                        signedValue = (short)aRegister;
                    }
                    else
                    {
                        signedValue = (short)(aRegister - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "BRG":
                    if (bRegister < 32768)
                    {
                        signedValue = (short)bRegister;
                    }
                    else
                    {
                        signedValue = (short)(bRegister - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "ACC":
                    if (cRegister < 32768)
                    {
                        signedValue = (short)cRegister;
                    }
                    else
                    {
                        signedValue = (short)(cRegister - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "XRG":
                    if (xRegister < 32768)
                    {
                        signedValue = (short)xRegister;
                    }
                    else
                    {
                        signedValue = (short)(xRegister - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "YRG":
                    if (yRegister < 32768)
                    {
                        signedValue = (short)yRegister;
                    }
                    else
                    {
                        signedValue = (short)(yRegister - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "STK":
                    if (stackPop() < 32768)
                    {
                        signedValue = (short)stackPop();
                    }
                    else
                    {
                        signedValue = (short)(stackPop() - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "CRM":
                    if (codeRam[operandValue] < 32768)
                    {
                        signedValue = (short)codeRam[operandValue];
                    }
                    else
                    {
                        signedValue = (short)(codeRam[operandValue] - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
                case "WRM":
                    if (workRam[operandValue] < 32768)
                    {
                        signedValue = (short)workRam[operandValue];
                    }
                    else
                    {
                        signedValue = (short)(workRam[operandValue] - 32768);
                    }
                    Console.WriteLine(signedValue);
                    break;
            }
            break;
        case "PCC":
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine("");
                    break;
                case "MAX":
                    Console.WriteLine("PCC MAX: function undefined");
                    break;
                case "ARB":
                    Console.WriteLine((char)operandValue);
                    break;
                case "ARG":
                    Console.WriteLine((char)aRegister);
                    break;
                case "BRG":
                    Console.WriteLine((char)bRegister);
                    break;
                case "ACC":
                    Console.WriteLine((char)cRegister);
                    break;
                case "XRG":
                    Console.WriteLine((char)xRegister);
                    break;
                case "YRG":
                    Console.WriteLine((char)yRegister);
                    break;
                case "STK":
                    Console.WriteLine((char)stackPop());
                    break;
                case "CRM":
                    Console.WriteLine((char)codeRam[operandValue]);
                    break;
                case "WRM":
                    Console.WriteLine((char)workRam[operandValue]);
                    break;
            }
            break;
        case "JIF":
            switch (suboperatorString)
            {
                case "MIN":
                    if (flag)
                    {
                        jumpTo = 0;
                    }
                    break;
                case "MAX":
                    if (flag)
                    {
                        jumpTo = 65535;
                    }
                    break;
                case "ARB":
                    if (flag)
                    {
                        jumpTo = aRegister;
                    }
                    break;
                case "ARG":
                    if (flag)
                    {
                        jumpTo = aRegister;
                    }
                    break;
                case "BRG":
                    if (flag)
                    {
                        jumpTo = bRegister;
                    }
                    break;
                case "ACC":
                    if (flag)
                    {
                        jumpTo = cRegister;
                    }
                    break;
                case "XRG":
                    if (flag)
                    {
                        jumpTo = xRegister;
                    }
                    break;
                case "YRG":
                    if (flag)
                    {
                        jumpTo = yRegister;
                    }
                    break;
                case "STK":
                    if (flag)
                    {
                        jumpTo = stackPop();
                    }
                    break;
                case "CRM":
                    if (flag)
                    {
                        jumpTo = codeRam[operandValue];
                    }
                    break;
                case "WRM":
                    if (flag)
                    {
                        jumpTo = workRam[operandValue];
                    }
                    break;
            }
            break;
        case "JNF":
            switch (suboperatorString)
            {
                case "MIN":
                    if (!flag)
                    {
                        jumpTo = 0;
                    }
                    break;
                case "MAX":
                    if (!flag)
                    {
                        jumpTo = 65535;
                    }
                    break;
                case "ARB":
                    if (!flag)
                    {
                        jumpTo = aRegister;
                    }
                    break;
                case "ARG":
                    if (!flag)
                    {
                        jumpTo = aRegister;
                    }
                    break;
                case "BRG":
                    if (!flag)
                    {
                        jumpTo = bRegister;
                    }
                    break;
                case "ACC":
                    if (!flag)
                    {
                        jumpTo = cRegister;
                    }
                    break;
                case "XRG":
                    if (!flag)
                    {
                        jumpTo = xRegister;
                    }
                    break;
                case "YRG":
                    if (!flag)
                    {
                        jumpTo = yRegister;
                    }
                    break;
                case "STK":
                    if (!flag)
                    {
                        jumpTo = stackPop();
                    }
                    break;
                case "CRM":
                    if (!flag)
                    {
                        jumpTo = codeRam[operandValue];
                    }
                    break;
                case "WRM":
                    if (!flag)
                    {
                        jumpTo = workRam[operandValue];
                    }
                    break;
            }
            break;
        case "LDA":
            switch (suboperatorString)
            {
                case "MIN":
                    aRegister = 0;
                    break;
                case "MAX":
                    aRegister = 65535;
                    break;
                case "ARB":
                    aRegister = operandValue;
                    break;
                case "BRG":
                    aRegister = bRegister;
                    break;
                case "ACC":
                    aRegister = cRegister;
                    break;
                case "XRG":
                    aRegister = xRegister;
                    break;
                case "YRG":
                    aRegister = yRegister;
                    break;
                case "STK":
                    aRegister = stackPop();
                    break;
                case "CRM":
                    aRegister = codeRam[operandValue];
                    break;
                case "WRM":
                    aRegister = workRam[operandValue];
                    break;
            }
            break;
        case "STA":
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine("STA MIN: function undefined");
                    break;
                case "MAX":
                    Console.WriteLine("STA MAX: function undefined");
                    break;
                case "ARB":
                    Console.WriteLine("STA ARB: function undefined");
                    break;
                case "BRG":
                    bRegister = aRegister;
                    break;
                case "ACC":
                    cRegister = aRegister;
                    break;
                case "XRG":
                    xRegister = aRegister;
                    break;
                case "YRG":
                    yRegister = aRegister;
                    break;
                case "STK":
                    stackPush(aRegister);
                    break;
                case "CRM":
                    codeRam[operandValue] = aRegister;
                    break;
                case "WRM":
                    workRam[operandValue] = aRegister;
                    break;
            }
            break;
        case "ATA":
            switch (suboperatorString)
            {
                case "MIN":
                    aRegister += 1;
                    break;
                case "MAX":
                    aRegister += 65535;
                    break;
                case "ARB":
                    aRegister += operandValue;
                    break;
                case "ARG":
                    aRegister += aRegister;
                    break;
                case "BRG":
                    aRegister += bRegister;
                    break;
                case "ACC":
                    aRegister += cRegister;
                    break;
                case "XRG":
                    aRegister += xRegister;
                    break;
                case "YRG":
                    aRegister += yRegister;
                    break;
                case "STK":
                    aRegister += stackPop();
                    break;
                case "CRM":
                    aRegister += codeRam[operandValue];
                    break;
                case "WRM":
                    aRegister += workRam[operandValue];
                    break;
            }
            break;
        case "SFA":
            switch (suboperatorString)
            {
                case "MIN":
                    aRegister -= 1;
                    break;
                case "MAX":
                    aRegister -= 65535;
                    break;
                case "ARB":
                    aRegister -= operandValue;
                    break;
                case "ARG":
                    aRegister = 0;
                    break;
                case "BRG":
                    aRegister -= bRegister;
                    break;
                case "ACC":
                    aRegister -= cRegister;
                    break;
                case "XRG":
                    aRegister -= xRegister;
                    break;
                case "YRG":
                    aRegister -= yRegister;
                    break;
                case "STK":
                    aRegister -= stackPop();
                    break;
                case "CRM":
                    aRegister -= codeRam[operandValue];
                    break;
                case "WRM":
                    aRegister -= workRam[operandValue];
                    break;
            }
            break;
        case "MBA":
            switch (suboperatorString)
            {
                case "MIN":
                    aRegister *= 2;
                    break;
                case "MAX":
                    aRegister *= 65535;
                    break;
                case "ARB":
                    aRegister *= operandValue;
                    break;
                case "ARG":
                    aRegister *= aRegister;
                    break;
                case "BRG":
                    aRegister *= bRegister;
                    break;
                case "ACC":
                    aRegister *= cRegister;
                    break;
                case "XRG":
                    aRegister *= xRegister;
                    break;
                case "YRG":
                    aRegister *= yRegister;
                    break;
                case "STK":
                    aRegister *= stackPop();
                    break;
                case "CRM":
                    aRegister *= codeRam[operandValue];
                    break;
                case "WRM":
                    aRegister *= workRam[operandValue];
                    break;
            }
            break;
        case "DAB":
            switch (suboperatorString)
            {
                case "MIN":
                    aRegister /= 2;
                    break;
                case "MAX":
                    aRegister /= 65535;
                    break;
                case "ARB":
                    aRegister /= operandValue;
                    break;
                case "ARG":
                    aRegister = 1;
                    break;
                case "BRG":
                    aRegister /= bRegister;
                    break;
                case "ACC":
                    aRegister /= cRegister;
                    break;
                case "XRG":
                    aRegister /= xRegister;
                    break;
                case "YRG":
                    aRegister /= yRegister;
                    break;
                case "STK":
                    aRegister /= stackPop();
                    break;
                case "CRM":
                    aRegister /= codeRam[operandValue];
                    break;
                case "WRM":
                    aRegister /= workRam[operandValue];
                    break;
            }
            break;
        case "MOA":
            switch (suboperatorString)
            {
                case "MIN":
                    aRegister %= 2;
                    break;
                case "MAX":
                    aRegister %= 65535;
                    break;
                case "ARB":
                    aRegister %= operandValue;
                    break;
                case "ARG":
                    aRegister = 0;
                    break;
                case "BRG":
                    aRegister %= bRegister;
                    break;
                case "ACC":
                    aRegister %= cRegister;
                    break;
                case "XRG":
                    aRegister %= xRegister;
                    break;
                case "YRG":
                    aRegister %= yRegister;
                    break;
                case "STK":
                    aRegister %= stackPop();
                    break;
                case "CRM":
                    aRegister %= codeRam[operandValue];
                    break;
                case "WRM":
                    aRegister %= workRam[operandValue];
                    break;
            }
            break;
        case "FAE":
            switch (suboperatorString)
            {
                case "MIN":
                    flag = aRegister == 0;
                    break;
                case "MAX":
                    flag = aRegister == 65535;
                    break;
                case "ARB":
                    flag = aRegister == operandValue;
                    break;
                case "ARG":
                    flag = true;
                    break;
                case "BRG":
                    flag = aRegister == bRegister;
                    break;
                case "ACC":
                    flag = aRegister == cRegister;
                    break;
                case "XRG":
                    flag = aRegister == xRegister;
                    break;
                case "YRG":
                    flag = aRegister == yRegister;
                    break;
                case "STK":
                    flag = aRegister == stackPop();
                    break;
                case "CRM":
                    flag = aRegister == codeRam[operandValue];
                    break;
                case "WRM":
                    flag = aRegister == workRam[operandValue];
                    break;
            }
            break;
        case "FAM":
            switch (suboperatorString)
            {
                case "MIN":
                    flag = aRegister > 0;
                    break;
                case "MAX":
                    flag = false;
                    break;
                case "ARB":
                    flag = aRegister > operandValue;
                    break;
                case "ARG":                    
                    flag = false;
                    break;
                case "BRG":
                    flag = aRegister > bRegister;
                    break;
                case "ACC":
                    flag = aRegister > cRegister;
                    break;
                case "XRG":
                    flag = aRegister > xRegister;
                    break;
                case "YRG":
                    flag = aRegister > yRegister;
                    break;
                case "STK":
                    flag = aRegister > stackPop();
                    break;
                case "CRM":
                    flag = aRegister > codeRam[operandValue];
                    break;
                case "WRM":
                    flag = aRegister > workRam[operandValue];
                    break;
            }
            break;
        case "FAL":
            switch (suboperatorString)
            {
                case "MIN":
                    flag = false;
                    break;
                case "MAX":
                    flag = aRegister < 65535;
                    break;
                case "ARB":
                    flag = aRegister < operandValue;
                    break;
                case "ARG":
                    flag = false;
                    break;
                case "BRG":
                    flag = aRegister < bRegister;
                    break;
                case "ACC":
                    flag = aRegister < cRegister;
                    break;
                case "XRG":
                    flag = aRegister < xRegister;
                    break;
                case "YRG":
                    flag = aRegister < yRegister;
                    break;
                case "STK":
                    flag = aRegister < stackPop();
                    break;
                case "CRM":
                    flag = aRegister < codeRam[operandValue];
                    break;
                case "WRM":
                    flag = aRegister < workRam[operandValue];
                    break;
            }
            break;
        case "LDB":
            switch (suboperatorString)
            {
                case "MIN":
                    bRegister = 0;
                    break;
                case "MAX":
                    bRegister = 65535;
                    break;
                case "ARB":
                    bRegister = operandValue;
                    break;
                case "ARG":
                    bRegister = aRegister;
                    break;
                case "ACC":
                    bRegister = cRegister;
                    break;
                case "XRG":
                    bRegister = xRegister;
                    break;
                case "YRG":
                    bRegister = yRegister;
                    break;
                case "STK":
                    bRegister = stackPop();
                    break;
                case "CRM":
                    bRegister = codeRam[operandValue];
                    break;
                case "WRM":
                    bRegister = workRam[operandValue];
                    break;
            }
            break;
        case "STB":
            switch (suboperatorString)
            {
                case "MIN":
                    Console.WriteLine("STB MIN: function undefined");
                    break;
                case "MAX":
                    Console.WriteLine("STB MAX: function undefined");
                    break;
                case "ARB":
                    Console.WriteLine("STB ARB: function undefined");
                    break;
                case "ARG":
                    aRegister = bRegister;
                    break;
                case "ACC":
                    cRegister = bRegister;
                    break;
                case "XRG":
                    xRegister = bRegister;
                    break;
                case "YRG":
                    yRegister = bRegister;
                    break;
                case "STK":
                    stackPush(bRegister);
                    break;
                case "CRM":
                    codeRam[operandValue] = bRegister;
                    break;
                case "WRM":
                    workRam[operandValue] = bRegister;
                    break;
            }
            break;
        case "ATB":
            switch (suboperatorString)
            {
                case "MIN":
                    bRegister += 1;
                    break;
                case "MAX":
                    bRegister += 65535;
                    break;
                case "ARB":
                    bRegister += operandValue;
                    break;
                case "ARG":
                    bRegister += aRegister;
                    break;
                case "BRG":
                    bRegister += bRegister;
                    break;
                case "ACC":
                    bRegister += cRegister;
                    break;
                case "XRG":
                    bRegister += xRegister;
                    break;
                case "YRG":
                    bRegister += yRegister;
                    break;
                case "STK":
                    bRegister += stackPop();
                    break;
                case "CRM":
                    bRegister += codeRam[operandValue];
                    break;
                case "WRM":
                    bRegister += workRam[operandValue];
                    break;
            }
            break;
        case "SFB":
            switch (suboperatorString)
            {
                case "MIN":
                    bRegister -= 1;
                    break;
                case "MAX":
                    bRegister -= 65535;
                    break;
                case "ARB":
                    bRegister -= operandValue;
                    break;
                case "ARG":
                    bRegister = aRegister;
                    break;
                case "BRG":
                    bRegister -= 0;
                    break;
                case "ACC":
                    bRegister -= cRegister;
                    break;
                case "XRG":
                    bRegister -= xRegister;
                    break;
                case "YRG":
                    bRegister -= yRegister;
                    break;
                case "STK":
                    bRegister -= stackPop();
                    break;
                case "CRM":
                    bRegister -= codeRam[operandValue];
                    break;
                case "WRM":
                    bRegister -= workRam[operandValue];
                    break;
            }
            break;
        case "MBB":
            switch (suboperatorString)
            {
                case "MIN":
                    bRegister *= 2;
                    break;
                case "MAX":
                    bRegister *= 65535;
                    break;
                case "ARB":
                    bRegister *= operandValue;
                    break;
                case "ARG":
                    bRegister *= aRegister;
                    break;
                case "BRG":
                    bRegister *= bRegister;
                    break;
                case "ACC":
                    bRegister *= cRegister;
                    break;
                case "XRG":
                    bRegister *= xRegister;
                    break;
                case "YRG":
                    bRegister *= yRegister;
                    break;
                case "STK":
                    bRegister *= stackPop();
                    break;
                case "CRM":
                    bRegister *= codeRam[operandValue];
                    break;
                case "WRM":
                    bRegister *= workRam[operandValue];
                    break;
            }
            break;
        case "DBB":
            switch (suboperatorString)
            {
                case "MIN":
                    bRegister /= 2;
                    break;
                case "MAX":
                    bRegister /= 65535;
                    break;
                case "ARB":
                    bRegister /= operandValue;
                    break;
                case "ARG":
                    bRegister = aRegister;
                    break;
                case "BRG":
                    bRegister = 1;
                    break;
                case "ACC":
                    bRegister /= cRegister;
                    break;
                case "XRG":
                    bRegister /= xRegister;
                    break;
                case "YRG":
                    bRegister /= yRegister;
                    break;
                case "STK":
                    bRegister /= stackPop();
                    break;
                case "CRM":
                    bRegister /= codeRam[operandValue];
                    break;
                case "WRM":
                    bRegister /= workRam[operandValue];
                    break;
            }
            break;
        case "MOB":
            switch (suboperatorString)
            {
                case "MIN":
                    bRegister %= 2;
                    break;
                case "MAX":
                    bRegister %= 65535;
                    break;
                case "ARB":
                    bRegister %= operandValue;
                    break;
                case "ARG":
                    bRegister %= aRegister;
                    break;
                case "BRG":
                    bRegister = 0;
                    break;
                case "ACC":
                    bRegister %= cRegister;
                    break;
                case "XRG":
                    bRegister %= xRegister;
                    break;
                case "YRG":
                    bRegister %= yRegister;
                    break;
                case "STK":
                    bRegister %= stackPop();
                    break;
                case "CRM":
                    bRegister %= codeRam[operandValue];
                    break;
                case "WRM":
                    bRegister %= workRam[operandValue];
                    break;
            }
            break;
        case "FBE":
            switch (suboperatorString)
            {
                case "MIN":
                    flag = bRegister == 0;
                    break;
                case "MAX":
                    flag = bRegister == 65535;
                    break;
                case "ARB":
                    flag = bRegister == operandValue;
                    break;
                case "ARG":
                    flag = bRegister == aRegister;
                    break;
                case "BRG":
                    flag = true;
                    break;
                case "ACC":
                    flag = bRegister == cRegister;
                    break;
                case "XRG":
                    flag = bRegister == xRegister;
                    break;
                case "YRG":
                    flag = bRegister == yRegister;
                    break;
                case "STK":
                    flag = bRegister == stackPop();
                    break;
                case "CRM":
                    flag = bRegister == codeRam[operandValue];
                    break;
                case "WRM":
                    flag = bRegister == workRam[operandValue];
                    break;
            }
            break;
        case "FBM":
            switch (suboperatorString)
            {
                case "MIN":
                    flag = bRegister > 0;
                    break;
                case "MAX":
                    flag = false;
                    break;
                case "ARB":
                    flag = bRegister > operandValue;
                    break;
                case "ARG":
                    flag = bRegister > aRegister;
                    break;
                case "BRG":
                    flag = false;
                    break;
                case "ACC":
                    flag = bRegister > cRegister;
                    break;
                case "XRG":
                    flag = bRegister > xRegister;
                    break;
                case "YRG":
                    flag = bRegister > yRegister;
                    break;
                case "STK":
                    flag = bRegister > stackPop();
                    break;
                case "CRM":
                    flag = bRegister > codeRam[operandValue];
                    break;
                case "WRM":
                    flag = bRegister > workRam[operandValue];
                    break;
            }
            break;
        case "FBL":
            switch (suboperatorString)
            {
                case "MIN":
                    flag = false;
                    break;
                case "MAX":
                    flag = bRegister < 65535;
                    break;
                case "ARB":
                    flag = bRegister < operandValue;
                    break;
                case "ARG":
                    flag = bRegister < aRegister;
                    break;
                case "BRG":
                    flag = false;
                    break;
                case "ACC":
                    flag = bRegister < cRegister;
                    break;
                case "XRG":
                    flag = bRegister < xRegister;
                    break;
                case "YRG":
                    flag = bRegister < yRegister;
                    break;
                case "STK":
                    flag = bRegister < stackPop();
                    break;
                case "CRM":
                    flag = bRegister < codeRam[operandValue];
                    break;
                case "WRM":
                    flag = bRegister < workRam[operandValue];
                    break;
            }
            break;
    }
    return jumpTo;
}

for (ushort i = 0; i < codeRam.Length; i++)
{
    codeRam[i] = instructionInput();
    if(codeRam[i] == 0)
    {
        break;
    }
}
for (ushort i = 0; i < codeRam.Length; i++)
{
    ushort jumpTo = 0;
    if (codeRam[i] == 0)
    {
        break;
    }
    else
    {
        jumpTo = instructionExecute(codeRam[i]);
        if (jumpTo != 0)
        {
            i = (ushort)(jumpTo - 1);
            jumpTo = 0;
        }
    }
}