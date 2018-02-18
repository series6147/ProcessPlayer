# ProcessPlayer

Samples:

For every console script you have to open cmd console and enter: processplayer.exe "scriptpath".

1. calculator1.console.json. Command: processplayer.exe "...\calculator1.console.json"

<img width="1103" alt="calculator1.console" src="https://raw.githubusercontent.com/series6147/ProcessPlayer-state-machine/master/ProcessPlayer/Samples/Images/calculator1.console.png?_sm_au_=iFVPpPWHtk7T66HJ">

```

{
	"Children": [
	{
		"Blank": {
		"ID": "start",
		"OnExecuteStarted": "//initialization
globals['operand'] = null;
globals['operator'] = null;
globals['result'] = null;",
		"OutgoingIDs": ["console.selector"]
		}
	},
	{
		"Decision": {
		"ID": "console.selector",
		"Condition": "if (this.IsConsole)
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
		"IgnoreCalls": ["buffer"],
		"OutgoingIDs": ["console.selector","end","number","operator","reset"],
		"TriggerMode": 1
		}
	},
	{
		"Decision": {
		"Condition": "var values = this.getInput()['console.selector'];
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
		"ID": "number",
		"OutgoingIDs": ["calculator","console.selector"]
		}
	},
	{
		"Decision": {
		"Condition": "var values = globals['result'] == null ? this.getOutput() : this.getInput()['console.selector'];
if (values != null && values.Length)
{
	globals['operator'] = values[0].Data;
	
	this.msg(toString(globals['result']) + ' ' + toString(globals['operator']) + ' ' + toString(globals['operand']));
	
	return ['calculator'];
}
return ['console.selector'];",
		"ID": "operator",
		"OutgoingIDs": ["calculator","console.selector"]
		}
	},
	{
		"Blank": {
		"ID": "calculator",
		"OnExecuteStarted": "if (globals['operand'] == null || globals['operator'] == null || globals['result'] == null)
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
		"OutgoingIDs": ["console.selector"],
		"TriggerMode": 1
		}
	},
	{
		"Blank": {
		"ID": "reset",
		"OnExecuteStarted": "//reset
globals['operand'] = null;
globals['operator'] = null;
globals['result'] = null;",
		"OutgoingIDs": ["console.selector"]
		}
	},
	{
		"Blank": {
		"ID": "end"
		}
	}
	]
}
```

2. Project Calculator.sln demonstrate how to bind ProcessPlayer with user interface

<img width="359" alt="calculator1.windows" src="https://raw.githubusercontent.com/series6147/ProcessPlayer-state-machine/master/ProcessPlayer/Samples/Images/calculator1.windows.png?_sm_au_=iFVPpPWHtk7T66HJ">

3. Project GUI.Container.sln demonstrate how to bind machine state and current GUI scene.

<img width="1234" alt="calculator1.windows" src="https://raw.githubusercontent.com/series6147/ProcessPlayer-state-machine/master/ProcessPlayer/Samples/Images/guiContainer.windows.png?_sm_au_=iFVPpPWHtk7T66HJ">
