
MCS = dmcs
OPT = -out:build/Heisen.exe
LIB_OPT = -out:build/HeisenLib.dll -t:library -debug -r:Mono.Tasklets.dll
LIB = -r:build/HeisenLib.dll -debug -r:Mono.Tasklets.dll
#-r:lib/Mono.Cecil.dll -r:lib/Mono.Debugger.Soft.dll
srcdir = src/

FILES = ${srcdir}ITestDriver.cs \
	${srcdir}TestFixture.cs \
	${srcdir}IReplayInformations.cs \
	${srcdir}InvalidTestFixtureException.cs \
	${srcdir}Main.cs \
	${srcdir}AssemblyDissecter.cs \
	./lib/Options.cs \
	${srcdir}Scheduler.cs \
	${srcdir}RuntimeManager.cs \
	${srcdir}HeisenThread.cs \
	${srcdir}HijackingTestDriver.cs

LIB_FILES = ${srcdir}AssertException.cs \
	${srcdir}TestAttributes.cs \
	${srcdir}Assert.cs

all: $(FILES) library
	mkdir -p build
	$(MCS) $(OPT) $(LIB) $(FILES)
#	cp lib/*.dll* build/

library: $(LIB_FILES)
	mkdir -p build
	$(MCS) $(LIB_OPT) $(LIB_FILES)