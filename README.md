# ProcessPlayer

Are you tired from complex and not flexible end-state machine solutions? For your attention submitted intuitive and very powerful end-state player which can turn your software development to nice game.

Samples:

For every console script you have to open cmd console and enter: processplayer.exe "scriptpath".

**1. calculator1.console.json. Command: processplayer.exe "...\calculator1.console.json"**

<img width="1103" alt="calculator1.console" src="https://raw.githubusercontent.com/series6147/ProcessPlayer-state-machine/master/ProcessPlayer/Samples/Images/calculator1.console.png?_sm_au_=iFVq4kfnFbLfSNJF">

```
{
	Children: [
	{
		Blank: {
		ID: "start",
		OnExecuteStarted: "//initialization
globals['operand'] = null;
globals['operator'] = null;
globals['result'] = null;",
		OutgoingIDs: ["console.selector"]
		}
	},
	{
		Decision: {
		ID: "console.selector",
		Condition: "if (this.IsConsole)
{
	Console.WriteLine('enter number or operator.');
	var text = Console.ReadLine();
	if (isNumeric(text))
	{
		this.setInput({ 'buffer': [value(text)] });
		return ['number'];
	}
	else
	{
		switch (text)
		{
		case '':
		case '/':
		case '*':
		case '-':
		case '+':
		{
			this.setInput({ 'buffer': [text] });
			return ['operator'];
		}
			break;
		case 'c':
		case 'C':
			return ['reset'];
			break;
		}
	}
	return ['console.selector'];
}",
		IgnoreCalls: ["buffer"],
		OutgoingIDs: ["console.selector","end","number","operator","reset"],
		TriggerMode: 1
		}
	},
	{
		Decision: {
		Condition: "var values = this.getInput()['console.selector'];
if (values != null && values.Length)
{
	if (globals['result'] == null)
	{
		globals['result'] = values[0].Data;
	}
	else
	{
		globals['operand'] = values[0].Data;
	}

	this.msg(toString(globals['result']) + ' ' + toString(globals['operator']) + ' ' + toString(globals['operand']));
	
	return ['calculator'];
}
return ['console.selector'];",
		ID: "number",
		OutgoingIDs: ["calculator","console.selector"]
		}
	},
	{
		Decision: {
		Condition: "var values = globals['result'] == null ? this.getOutput() : this.getInput()['console.selector'];
if (values != null && values.Length)
{
	globals['operator'] = values[0].Data;
	
	this.msg(toString(globals['result']) + ' ' + toString(globals['operator']) + ' ' + toString(globals['operand']));
	
	return ['calculator'];
}
return ['console.selector'];",
		ID: "operator",
		OutgoingIDs: ["calculator","console.selector"]
		}
	},
	{
		Blank: {
		ID: "calculator",
		OnExecuteStarted: "if (globals['operand'] == null || globals['operator'] == null || globals['result'] == null)
{
	return;
}

switch (globals['operator'])
{
	case '/':
	{
		globals['result'] = globals['result'] / globals['operand'];
	}
		break;
	case '*':
	{
		globals['result'] = globals['result'] * globals['operand'];
	}
		break;
	case '-':
	{
		globals['result'] = globals['result'] - globals['operand'];
	}
		break;
	case '+':
	{
		globals['result'] = globals['result'] + globals['operand'];
	}
		break;
}

globals['operand'] = null;

this.msg('= ' + toString(globals['result']));
",
		OutgoingIDs: ["console.selector"],
		TriggerMode: 1
		}
	},
	{
		Blank: {
		ID: "reset",
		OnExecuteStarted: "//reset
globals['operand'] = null;
globals['operator'] = null;
globals['result'] = null;",
		OutgoingIDs: ["console.selector"]
		}
	},
	{
		Blank: {
		ID: "end"
		}
	}
	]
}
```

**2. Project Calculator.sln demonstrate how to bind ProcessPlayer with user interface**

<img width="359" alt="calculator1.windows" src="https://raw.githubusercontent.com/series6147/ProcessPlayer-state-machine/master/ProcessPlayer/Samples/Images/calculator1.windows.png?_sm_au_=iFVq4kfnFbLfSNJF">

```
{
	Children: [
	{
		Blank: {
		ID: "start",
		OnExecuteStarted: "//initialization
globals['operand'] = null;
globals['operator'] = null;
globals['result'] = null;
globals['texthistory'] = null;
globals['textnumber'] = null;",
		OutgoingIDs: ["selector"]
		}
	},
	{
		Selector: {
		ID: "selector",
		OutgoingIDs: ["selector","0","1","2","3","4","5","6","7","8","9","/","*","+","-","=","C","end"],
		TriggerMode: 1
		}
	},
	{
		Blank: {
		ID: "0",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 0;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "1",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 1;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "2",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 2;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "3",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 3;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "4",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 4;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "5",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 5;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "6",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 6;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "7",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 7;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "8",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 8;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Blank: {
		ID: "9",
		OnExecuteStarted: "globals['operand'] = toDouble(globals['operand']) * 10 + 9;",
		OutgoingIDs: ["number.text.update"]
		}
	},
	{
		Any: {
		ID: "number.text.update",
		OnExecuteStarted: "if (globals['operator'] == '=')
	globals['result'] = globals['operand'];
	
if (globals['result'] == null)
	globals['texthistory'] = toString(globals['operand']);
else if (globals['operand'] != null && globals['operator'] != '=')
{
	globals['texthistory'] = toString(globals['result']) + ' ' + globals['operator'] + ' ' + toString(globals['operand']);
}
else if (globals['operator'] != '=')
{
	globals['texthistory'] = toString(globals['result']) + ' ' + globals['operator'];
}
else
{
	globals['texthistory'] = toString(globals['result']);
}
globals['textnumber'] = toString(globals['operand']);",
		OutgoingIDs: ["selector"]
		}
	},
	{
		Blank: {
		ID: "/",
		OnExecuteStarted: "globals['operator'] = '/';",
		OutgoingIDs: ["calculator"]
		}
	},
	{
		Blank: {
		ID: "*",
		OnExecuteStarted: "globals['operator'] = '*';",
		OutgoingIDs: ["calculator"]
		}
	},
	{
		Blank: {
		ID: "+",
		OnExecuteStarted: "globals['operator'] = '+';",
		OutgoingIDs: ["calculator"]
		}
	},
	{
		Blank: {
		ID: "-",
		OnExecuteStarted: "globals['operator'] = '-';",
		OutgoingIDs: ["calculator"]
		}
	},
	{
		Blank: {
		ID: "=",
		OnExecuteStarted: "globals['operator'] = '=';",
		OutgoingIDs: ["calculator"]
		}
	},
	{
		Any: {
		ID: "calculator",
		OnExecuteStarted: "if (vars['operator'] == '=')
	globals['operand'] = null;
	
if (globals['operand'] != null && globals['result'] == null)
{
	globals['texthistory'] = toString(globals['operand']) + ' ' + globals['operator'];
}
else if (globals['operator'] != '=')
{
	globals['texthistory'] = toString(globals['result']) + ' ' + globals['operator'] + ' ' + toString(globals['operand']);
}

if (globals['operand'] != null && globals['result'] == null)
{
	globals['result'] = globals['operand'];
	globals['operand'] = null;
	vars['operator'] = globals['operator'];
	return;
}

switch (vars['operator'])
{
	case '/':
		globals['result'] = globals['result'] / globals['operand'];
		break;
	case '*':
		globals['result'] = globals['result'] * globals['operand'];
		break;
	case '-':
		globals['result'] = globals['result'] - globals['operand'];
		break;
	case '+':
		globals['result'] = globals['result'] + globals['operand'];
		break;
}

globals['operand'] = null;
globals['textnumber'] = toString(globals['result']);
vars['operator'] = globals['operator'];",
		OutgoingIDs: ["selector"]
		}
	},
	{
		Blank: {
		ID: "C",
		OnExecuteStarted: "//reset
globals['operand'] = null;
globals['operator'] = null;
globals['result'] = null;
globals['texthistory'] = null;
globals['textnumber'] = null;

var cnt = this.getContentByID('calculator');

if (cnt != null)
	cnt.Vars['operator'] = null;",
		OutgoingIDs: ["selector"]
		}
	},
	{
		Blank: {
		ID: "end"
		}
	}
	]
}
```

**3. Project GUI.Container.sln demonstrate how to bind machine state and current GUI scene.**

<img width="1234" alt="calculator1.windows" src="https://raw.githubusercontent.com/series6147/ProcessPlayer-state-machine/master/ProcessPlayer/Samples/Images/guiContainer.windows.png?_sm_au_=iFVq4kfnFbLfSNJF">

```
{
	Children: [
	{
		Blank: {
		ID: "start",
		OutgoingIDs: ["page1","page2"]
		}
	},
	{
		Group: {
		Children: [
			{
				Blank: {
				ID: "page1.1",
				OutgoingIDs: ["toggle1.1"],
				Views: ["Page1_1"]
				}
			},
			{
				CommandButton: {
				ID: "toggle1.1",
				OutgoingIDs: ["page1.2"]
				}
			},
			{
				Blank: {
				ID: "page1.2",
				OutgoingIDs: ["toggle1.2"],
				Views: ["Page1_2"]
				}
			},
			{
				CommandButton: {
				ID: "toggle1.2",
				OutgoingIDs: ["page1.3"]
				}
			},
			{
				Blank: {
				ID: "page1.3",
				OutgoingIDs: ["toggle1.3"],
				Views: ["Page1_3"]
				}
			},
			{
				CommandButton: {
				ID: "toggle1.3"
				}
			}
		],
		ID: "page1",
		OutgoingIDs: ["page1"],
		TriggerMode: 1
		}
	},
	{
		Group: {
		Children: [
			{
				Blank: {
				ID: "page2.1",
				OutgoingIDs: ["toggle2.1"],
				Views: ["Page2_1"]
				}
			},
			{
				CommandButton: {
				ID: "toggle2.1",
				OutgoingIDs: ["page2.2"]
				}
			},
			{
				Blank: {
				ID: "page2.2",
				OutgoingIDs: ["toggle2.2"],
				Views: ["Page2_2"]
				}
			},
			{
				CommandButton: {
				ID: "toggle2.2",
				OutgoingIDs: ["page2.3"]
				}
			},
			{
				Blank: {
				ID: "page2.3",
				OutgoingIDs: ["toggle2.3"],
				Views: ["Page2_3"]
				}
			},
			{
				CommandButton: {
				ID: "toggle2.3"
				}
			}
		],
		ID: "page2",
		OutgoingIDs: ["page2"],
		TriggerMode: 1
		}
	},
	{
		ToggleSwitch: {
		ID: "end"
		}
	}
	]
}
```
