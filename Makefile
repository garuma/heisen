
MCS = dmcs
OPT = -out:build/Heisen.exe
LIB_OPT = -out:build/HeisenLib.dll -t:library -debug
LIB = -r:lib/Mono.Cecil.dll -r:lib/Mono.Debugger.Soft.dll -r:build/HeisenLib.dll -debug

FILES = src/ITestDriver.cs \
	src/TestFixture.cs \
	src/ReplayInformations.cs \
	src/InvalidTestFixtureException.cs \
	src/Main.cs \
	src/HeisenFuzzer.cs \
	src/AssemblyDissecter.cs \
	./lib/Options.cs \
	src/Scheduler.cs \
	src/RuntimeManager.cs \
	src/HeisenThread.cs \
	src/Continuation.cs


LIB_FILES = src/IHeisenTestFixture.cs \
	src/AssertException.cs \
	src/HeisenThread.cs \
	src/Scheduler.cs \
	src/RuntimeManager.cs \
	src/Continuation.cs

all: $(FILES) library
	mkdir -p build
	$(MCS) $(OPT) $(LIB) $(FILES)
	cp lib/*.dll* build/

library: $(LIB_FILES)
	mkdir -p build
	$(MCS) $(LIB_OPT) $(LIB_FILES)