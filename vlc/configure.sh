#!/bin/sh

OPTIONS="
      --enable-winstore-app
      --enable-lua
      --disable-vlm --disable-sout
      --disable-faad
      --disable-flac
      --enable-theora
      --disable-twolame
      --enable-quicktime
      --enable-avcodec --enable-merge-ffmpeg
      --enable-dca
      --enable-mpc
      --enable-libass
      --disable-x264
      --enable-schroedinger
      --enable-realrtsp
      --enable-live555
      --enable-shout
      --disable-goom
      --disable-caca
      --disable-sdl
      --disable-qt
      --disable-skins2
      --enable-sse --enable-mmx
      --disable-libcddb
      --disable-zvbi --disable-telx
      --disable-sqlite
      --disable-mad
      --disable-a52
      --enable-vlc
      --disable-libgcrypt
      --disable-dirac"

if gcc -v 2>/dev/null -a echo | gcc -mno-cygwin -E -2>/dev/null 2>&1
then
    echo Cygwin detected, adjusting options
    export CC="gcc -mno-cygwin"
    export CXX="g++ -mno-cygwin"
    OPTIONS="${OPTIONS} --disable-taglib --disable-mkv"
fi

# We want the getpid replacement (GetCurrentProcessId), since getpid() is wrong on Windows.
ac_cv_func_getpid=no \ 
 sh ../configure ${OPTIONS} $*
