
ANDROID_NDK_HOME = ~/android-ndk-r17b
ANDROID_ABIS = armeabi-v7a arm64-v8a x86 x86_64

all:
	for abi in $(ANDROID_ABIS); do \
		mkdir -p build/oboe/$$abi ; \
		cd build/oboe/$$abi ; \
		cmake -DCMAKE_TOOLCHAIN_FILE=$(ANDROID_NDK_HOME)/build/cmake/android.toolchain.cmake -DANDROID_NDK=$(ANDROID_NDK_HOME) -DCMAKE_BUILD_TYPE=Debug -DANDROID_ABI=$$abi ../../../external/oboe ; \
		make ; \
		cd ../../.. ; \
		mkdir -p build/oboe-c/$$abi ; \
		cd build/oboe-c/$$abi ; \
		cmake -DCMAKE_TOOLCHAIN_FILE=$(ANDROID_NDK_HOME)/build/cmake/android.toolchain.cmake -DANDROID_NDK=$(ANDROID_NDK_HOME) -DCMAKE_BUILD_TYPE=Debug -DANDROID_ABI=$$abi ../../../native ; \
		make ; \
		cd ../../.. ; \
	done

