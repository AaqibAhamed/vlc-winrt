/*****************************************************************************
* EqualizerCX.hpp: Equalizer API
*****************************************************************************
* Copyright © 2015 libvlcpp authors & VideoLAN
*
* Authors: Hugo Beauzée-Luyssen <hugo@beauzee.fr>
*          Bastien Penavayre <bastienPenava@gmail.com>
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as published by
* the Free Software Foundation; either version 2.1 of the License, or
* (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General Public License
* along with this program; if not, write to the Free Software
* Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston MA 02110-1301, USA.
*****************************************************************************/

#pragma once

#include "../libvlcpp/vlcpp/Equalizer.hpp"

namespace libVLCX
{

    public ref class Equalizer sealed
    {
    internal:
        VLC::Equalizer m_eq;

    public:
        /**
        * Create a new default equalizer, with all frequency values zeroed.
        *
        * The new equalizer can subsequently be applied to a media player by invoking
        * libvlc_media_player_set_equalizer().
        *
        * \throw std::runtime_error when equalizer creation fails
        *
        * \version LibVLC 2.2.0 or later
        */
        Equalizer();

        /**
        * Create a new equalizer, with initial frequency values copied from an existing
        * preset.
        *
        * The new equalizer can subsequently be applied to a media player by invoking
        * libvlc_media_player_set_equalizer().
        *
        * \param index index of the preset, counting from zero
        *
        * \throw std::runtime_error when equalizer creation fails
        *
        * \version LibVLC 2.2.0 or later
        */
        Equalizer(unsigned int index);

        /**
        * Set a new pre-amplification value for an equalizer.
        *
        * The new equalizer settings are subsequently applied to a media player by invoking
        * MediaPlayer::setEqualizer().
        *
        * The supplied amplification value will be clamped to the -20.0 to +20.0 range.
        *
        * \param preamp preamp value (-20.0 to 20.0 Hz)
        * \return zero on success, -1 on error
        * \version LibVLC 2.2.0 or later
        */
        int setPreamp(float preamp);

        /**
        * Get the current pre-amplification value from an equalizer.
        *
        * \return preamp value (Hz)
        * \version LibVLC 2.2.0 or later
        */
        float preamp();

        /**
        * Set a new amplification value for a particular equalizer frequency band.
        *
        * The new equalizer settings are subsequently applied to a media player by invoking
        * MediaPlayer::setEqualizer().
        *
        * The supplied amplification value will be clamped to the -20.0 to +20.0 range.
        *
        * \param amp amplification value (-20.0 to 20.0 Hz)
        * \param band index, counting from zero, of the frequency band to set
        * \return zero on success, -1 on error
        * \version LibVLC 2.2.0 or later
        */
        int setAmp(float amp, unsigned int band);

        /**
        * Get the amplification value for a particular equalizer frequency band.
        *
        * \param u_band index, counting from zero, of the frequency band to get
        * \return amplification value (Hz); NaN if there is no such frequency band
        * \version LibVLC 2.2.0 or later
        */
        float amp(unsigned int band);

        /**
        * Get the number of equalizer presets.
        *
        * \return number of presets
        * \version LibVLC 2.2.0 or later
        */
        static unsigned int presetCount();

        /**
        * Get the name of a particular equalizer preset.
        *
        * This name can be used, for example, to prepare a preset label or menu in a user
        * interface.
        *
        * \param index index of the preset, counting from zero
        * \return preset name, or empty string if there is no such preset
        * \version LibVLC 2.2.0 or later
        */
        static Platform::String^ presetName(unsigned index);

        /**
        * Get the number of distinct frequency bands for an equalizer.
        *
        * \return number of frequency bands
        * \version LibVLC 2.2.0 or later
        */
        static unsigned int bandCount();

        /**
        * Get a particular equalizer band frequency.
        *
        * This value can be used, for example, to create a label for an equalizer band control
        * in a user interface.
        *
        * \param index index of the band, counting from zero
        * \return equalizer band frequency (Hz), or -1 if there is no such band
        * \version LibVLC 2.2.0 or later
        */
        static float bandFrequency(unsigned int index);
    };
}

