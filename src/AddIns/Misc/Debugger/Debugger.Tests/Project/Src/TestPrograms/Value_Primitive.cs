﻿// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbeck�" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

using System;

namespace Debugger.Tests.TestPrograms
{
	public class Value_Primitive
	{
		public static void Main()
		{
			bool b = true;
			int i = 5;
			string s = "five";
			double d = 5.5;
			System.Diagnostics.Debugger.Break();
		}
	}
}

#if TEST_CODE
namespace Debugger.Tests {
	public partial class DebuggerTests
	{
		[NUnit.Framework.Test]
		public void Value_Primitive()
		{
			ExpandProperties(
				"Value.Type",
				"DebugType.BaseType"
			);
			StartTest("Value_Primitive.cs");
			
			ObjectDump("locals", process.SelectedStackFrame.GetLocalVariableValues());
			// Test System.Object access
			ObjectDump("b as string", process.SelectedStackFrame.GetLocalVariableValue("b").InvokeToString());
			ObjectDump("i as string", process.SelectedStackFrame.GetLocalVariableValue("i").InvokeToString());
			ObjectDump("s as string", process.SelectedStackFrame.GetLocalVariableValue("s").InvokeToString());
			
			EndTest();
		}
	}
}
#endif

#if EXPECTED_OUTPUT
<?xml version="1.0" encoding="utf-8"?>
<DebuggerTests>
  <Test
    name="Value_Primitive.cs">
    <ProcessStarted />
    <ModuleLoaded>mscorlib.dll (No symbols)</ModuleLoaded>
    <ModuleLoaded>Value_Primitive.exe (Has symbols)</ModuleLoaded>
    <DebuggingPaused>Break Value_Primitive.cs:20,4-20,40</DebuggingPaused>
    <locals
      Capacity="4"
      Count="4">
      <Item>
        <Value
          AsString="True"
          PrimitiveValue="True"
          Type="System.Boolean">
          <Type>
            <DebugType
              BaseType="System.Object"
              FullName="System.Boolean"
              Kind="Primitive"
              Module="{Exception: The type is not a class or value type.}"
              Name="Boolean">
              <BaseType>
                <DebugType
                  FullName="System.Object"
                  Kind="Class"
                  Module="mscorlib.dll"
                  Name="Object">
                  <BaseType>null</BaseType>
                </DebugType>
              </BaseType>
            </DebugType>
          </Type>
        </Value>
      </Item>
      <Item>
        <Value
          AsString="5"
          PrimitiveValue="5"
          Type="System.Int32">
          <Type>
            <DebugType
              BaseType="System.Object"
              FullName="System.Int32"
              Kind="Primitive"
              Module="{Exception: The type is not a class or value type.}"
              Name="Int32">
              <BaseType>
                <DebugType
                  FullName="System.Object"
                  Kind="Class"
                  Module="mscorlib.dll"
                  Name="Object">
                  <BaseType>null</BaseType>
                </DebugType>
              </BaseType>
            </DebugType>
          </Type>
        </Value>
      </Item>
      <Item>
        <Value
          AsString="five"
          IsReference="True"
          PrimitiveValue="five"
          Type="System.String">
          <Type>
            <DebugType
              BaseType="System.Object"
              FullName="System.String"
              Kind="Primitive"
              Module="{Exception: The type is not a class or value type.}"
              Name="String">
              <BaseType>
                <DebugType
                  FullName="System.Object"
                  Kind="Class"
                  Module="mscorlib.dll"
                  Name="Object">
                  <BaseType>null</BaseType>
                </DebugType>
              </BaseType>
            </DebugType>
          </Type>
        </Value>
      </Item>
      <Item>
        <Value
          AsString="5.5"
          PrimitiveValue="5.5"
          Type="System.Double">
          <Type>
            <DebugType
              BaseType="System.Object"
              FullName="System.Double"
              Kind="Primitive"
              Module="{Exception: The type is not a class or value type.}"
              Name="Double">
              <BaseType>
                <DebugType
                  FullName="System.Object"
                  Kind="Class"
                  Module="mscorlib.dll"
                  Name="Object">
                  <BaseType>null</BaseType>
                </DebugType>
              </BaseType>
            </DebugType>
          </Type>
        </Value>
      </Item>
    </locals>
    <b_as_string>True</b_as_string>
    <i_as_string>5</i_as_string>
    <s_as_string>five</s_as_string>
    <ProcessExited />
  </Test>
</DebuggerTests>
#endif // EXPECTED_OUTPUT