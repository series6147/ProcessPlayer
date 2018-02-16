# ProcessPlayer

Samples:

For every console script you have to open cmd console and enter: processplayer.exe "scriptpath".

1. calculator1.console.json. Command: processplayer.exe "...\calculator1.console.json"

<img width="1103" alt="calculator1.console" src="https://raw.githubusercontent.com/series6147/ProcessPlayer/master/ProcessPlayer/Samples/Images/calculator1.console.png?_sm_au_=iFVk6V7Fs1qN43fs">

{::nomarkdown}

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/1999/REC-html401-19991224/strict.dtd">
<html>
<head>
<META http-equiv=Content-Type content="text/html; charset=UTF-8">
<title>Exported from Notepad++</title>
<style type="text/css">
span {
	font-family: 'Courier New';
	font-size: 10pt;
	color: #000000;
}
.sc0 {
}
.sc4 {
	color: #FF8000;
}
.sc5 {
	font-weight: bold;
	color: #0000FF;
}
.sc6 {
	color: #800000;
}
.sc7 {
	color: #808080;
}
.sc10 {
	font-weight: bold;
	color: #8000FF;
}
.sc11 {
}
.sc12 {
}
</style>
</head>
<body>
<div style="float: left; white-space: pre; line-height: 1; background: #FFFFFF; "><span class="sc10">{</span><span class="sc0">
    </span><span class="sc11">Children</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Blank</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">ID</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc6">"start"</span><span class="sc10">,</span><span class="sc0">
        </span><span class="sc11">OnExecuteStarted</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc12">"//initialization
</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">;</span><span class="sc0">
</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operator'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">;</span><span class="sc0">
</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">;</span><span class="sc12">",
</span><span class="sc0">        </span><span class="sc11">OutgoingIDs</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc6">"console.selector"</span><span class="sc10">]</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">},</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Decision</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">ID</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc6">"console.selector"</span><span class="sc10">,</span><span class="sc0">
        </span><span class="sc11">Condition</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc12">"if (this.IsConsole)
</span><span class="sc10">{</span><span class="sc0">
    </span><span class="sc11">Console</span><span class="sc10">.</span><span class="sc11">WriteLine</span><span class="sc10">(</span><span class="sc7">'enter number or operator.'</span><span class="sc10">);</span><span class="sc0">
    </span><span class="sc11">var</span><span class="sc0"> </span><span class="sc11">text</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">Console</span><span class="sc10">.</span><span class="sc11">ReadLine</span><span class="sc10">();</span><span class="sc0">
    </span><span class="sc11">if</span><span class="sc0"> </span><span class="sc10">(</span><span class="sc11">isNumeric</span><span class="sc10">(</span><span class="sc11">text</span><span class="sc10">))</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">this</span><span class="sc10">.</span><span class="sc11">setInput</span><span class="sc10">({</span><span class="sc0"> </span><span class="sc7">'buffer'</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc11">value</span><span class="sc10">(</span><span class="sc11">text</span><span class="sc10">)]</span><span class="sc0"> </span><span class="sc10">});</span><span class="sc0">
        </span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'number'</span><span class="sc10">];</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc11">else</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">switch</span><span class="sc0"> </span><span class="sc10">(</span><span class="sc11">text</span><span class="sc10">)</span><span class="sc0">
        </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">''</span><span class="sc10">:</span><span class="sc0">
        </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'/'</span><span class="sc10">:</span><span class="sc0">
        </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'*'</span><span class="sc10">:</span><span class="sc0">
        </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'-'</span><span class="sc10">:</span><span class="sc0">
        </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'+'</span><span class="sc10">:</span><span class="sc0">
        </span><span class="sc10">{</span><span class="sc0">
            </span><span class="sc11">this</span><span class="sc10">.</span><span class="sc11">setInput</span><span class="sc10">({</span><span class="sc0"> </span><span class="sc7">'buffer'</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc11">text</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">});</span><span class="sc0">
            </span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'operator'</span><span class="sc10">];</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
            </span><span class="sc11">break</span><span class="sc10">;</span><span class="sc0">
        </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'c'</span><span class="sc10">:</span><span class="sc0">
        </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'C'</span><span class="sc10">:</span><span class="sc0">
            </span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'reset'</span><span class="sc10">];</span><span class="sc0">
            </span><span class="sc11">break</span><span class="sc10">;</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'console.selector'</span><span class="sc10">];</span><span class="sc0">
</span><span class="sc10">}</span><span class="sc12">",
</span><span class="sc0">        </span><span class="sc11">IgnoreCalls</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc6">"buffer"</span><span class="sc10">],</span><span class="sc0">
        </span><span class="sc11">OutgoingIDs</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc6">"console.selector"</span><span class="sc10">,</span><span class="sc6">"end"</span><span class="sc10">,</span><span class="sc6">"number"</span><span class="sc10">,</span><span class="sc6">"operator"</span><span class="sc10">,</span><span class="sc6">"reset"</span><span class="sc10">],</span><span class="sc0">
        </span><span class="sc11">TriggerMode</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc4">1</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">},</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Decision</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Condition</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc12">"var values = this.getInput()['console.selector'];
</span><span class="sc11">if</span><span class="sc0"> </span><span class="sc10">(</span><span class="sc11">values</span><span class="sc0"> </span><span class="sc10">!=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc0"> </span><span class="sc10">&amp;&amp;</span><span class="sc0"> </span><span class="sc11">values</span><span class="sc10">.</span><span class="sc11">Length</span><span class="sc10">)</span><span class="sc0">
</span><span class="sc10">{</span><span class="sc0">
    </span><span class="sc11">if</span><span class="sc0"> </span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">==</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">)</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">values</span><span class="sc10">[</span><span class="sc4">0</span><span class="sc10">].</span><span class="sc11">Data</span><span class="sc10">;</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc11">else</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">values</span><span class="sc10">[</span><span class="sc4">0</span><span class="sc10">].</span><span class="sc11">Data</span><span class="sc10">;</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">

    </span><span class="sc11">this</span><span class="sc10">.</span><span class="sc11">msg</span><span class="sc10">(</span><span class="sc11">toString</span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">])</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc7">' '</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc11">toString</span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operator'</span><span class="sc10">])</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc7">' '</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc11">toString</span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">]));</span><span class="sc0">
    
    </span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'calculator'</span><span class="sc10">];</span><span class="sc0">
</span><span class="sc10">}</span><span class="sc0">
</span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'console.selector'</span><span class="sc10">];</span><span class="sc12">",
</span><span class="sc0">        </span><span class="sc11">ID</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc6">"number"</span><span class="sc10">,</span><span class="sc0">
        </span><span class="sc11">OutgoingIDs</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc6">"calculator"</span><span class="sc10">,</span><span class="sc6">"console.selector"</span><span class="sc10">]</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">},</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Decision</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Condition</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc12">"var values = globals['result'] == null ? this.getOutput() : this.getInput()['console.selector'];
</span><span class="sc11">if</span><span class="sc0"> </span><span class="sc10">(</span><span class="sc11">values</span><span class="sc0"> </span><span class="sc10">!=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc0"> </span><span class="sc10">&amp;&amp;</span><span class="sc0"> </span><span class="sc11">values</span><span class="sc10">.</span><span class="sc11">Length</span><span class="sc10">)</span><span class="sc0">
</span><span class="sc10">{</span><span class="sc0">
    </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operator'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">values</span><span class="sc10">[</span><span class="sc4">0</span><span class="sc10">].</span><span class="sc11">Data</span><span class="sc10">;</span><span class="sc0">
    
    </span><span class="sc11">this</span><span class="sc10">.</span><span class="sc11">msg</span><span class="sc10">(</span><span class="sc11">toString</span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">])</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc7">' '</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc11">toString</span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operator'</span><span class="sc10">])</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc7">' '</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc11">toString</span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">]));</span><span class="sc0">
    
    </span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'calculator'</span><span class="sc10">];</span><span class="sc0">
</span><span class="sc10">}</span><span class="sc0">
</span><span class="sc11">return</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc7">'console.selector'</span><span class="sc10">];</span><span class="sc12">",
</span><span class="sc0">        </span><span class="sc11">ID</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc6">"operator"</span><span class="sc10">,</span><span class="sc0">
        </span><span class="sc11">OutgoingIDs</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc6">"calculator"</span><span class="sc10">,</span><span class="sc6">"console.selector"</span><span class="sc10">]</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">},</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Blank</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">ID</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc6">"calculator"</span><span class="sc10">,</span><span class="sc0">
        </span><span class="sc11">OnExecuteStarted</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc12">"if (globals['operand'] == null || globals['operator'] == null || globals['result'] == null)
</span><span class="sc10">{</span><span class="sc0">
    </span><span class="sc11">return</span><span class="sc10">;</span><span class="sc0">
</span><span class="sc10">}</span><span class="sc0">

</span><span class="sc11">switch</span><span class="sc0"> </span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operator'</span><span class="sc10">])</span><span class="sc0">
</span><span class="sc10">{</span><span class="sc0">
    </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'/'</span><span class="sc10">:</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">/</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">];</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
        </span><span class="sc11">break</span><span class="sc10">;</span><span class="sc0">
    </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'*'</span><span class="sc10">:</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">*</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">];</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
        </span><span class="sc11">break</span><span class="sc10">;</span><span class="sc0">
    </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'-'</span><span class="sc10">:</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">-</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">];</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
        </span><span class="sc11">break</span><span class="sc10">;</span><span class="sc0">
    </span><span class="sc11">case</span><span class="sc0"> </span><span class="sc7">'+'</span><span class="sc10">:</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">];</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
        </span><span class="sc11">break</span><span class="sc10">;</span><span class="sc0">
</span><span class="sc10">}</span><span class="sc0">

</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">;</span><span class="sc0">

</span><span class="sc11">this</span><span class="sc10">.</span><span class="sc11">msg</span><span class="sc10">(</span><span class="sc7">'= '</span><span class="sc0"> </span><span class="sc10">+</span><span class="sc0"> </span><span class="sc11">toString</span><span class="sc10">(</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]));</span><span class="sc0">
</span><span class="sc12">",
</span><span class="sc0">        </span><span class="sc11">OutgoingIDs</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc6">"console.selector"</span><span class="sc10">],</span><span class="sc0">
        </span><span class="sc11">TriggerMode</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc4">1</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">},</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Blank</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">ID</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc6">"reset"</span><span class="sc10">,</span><span class="sc0">
        </span><span class="sc11">OnExecuteStarted</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc12">"//reset
</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operand'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">;</span><span class="sc0">
</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'operator'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">;</span><span class="sc0">
</span><span class="sc11">globals</span><span class="sc10">[</span><span class="sc7">'result'</span><span class="sc10">]</span><span class="sc0"> </span><span class="sc10">=</span><span class="sc0"> </span><span class="sc5">null</span><span class="sc10">;</span><span class="sc12">",
</span><span class="sc0">        </span><span class="sc11">OutgoingIDs</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">[</span><span class="sc6">"console.selector"</span><span class="sc10">]</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">},</span><span class="sc0">
    </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">Blank</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc10">{</span><span class="sc0">
        </span><span class="sc11">ID</span><span class="sc10">:</span><span class="sc0"> </span><span class="sc6">"end"</span><span class="sc0">
        </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">}</span><span class="sc0">
    </span><span class="sc10">]</span><span class="sc0">
</span><span class="sc10">}</span></div></body>
</html>

{:/}

2. Project Calculator.sln demonstrate how to bind ProcessPlayer with user interface

<img width="359" alt="calculator1.windows" src="https://raw.githubusercontent.com/series6147/ProcessPlayer/master/ProcessPlayer/Samples/Images/calculator1.windows.png?_sm_au_=iFVk6V7Fs1qN43fs">
