import { getVideoDevices, startVideo, toggleCamera, attachTrack, detachTrack } from "./video.js";
import { getAudioDevices, setMicrophone, toggleMicrophone } from "./audio.js";
import { isMemberDefined } from "./isMemberDefined.js";

let _participants = new Map();
let _dominantSpeaker = null;

async function createOrJoinRoom(roomName, token) {
    try {
        if (window.activeRoom) {
            window.activeRoom.disconnect();
        }

        const audioTrack = await Twilio.Video.createLocalAudioTrack();
        const tracks = [audioTrack, window.videoTrack];
        try {
            console.log("Connecting to the room...");
            window.activeRoom = await Twilio.Video.connect(token, {
                name: roomName,
                tracks,
                dominantSpeaker: true
            });
        } catch (error) {
            console.error("Failed to connect:", error);
        }

        if (window.activeRoom) {
            initialize(window.activeRoom.participants);
            window.activeRoom
                .on('disconnected',
                    room => room.localParticipant.tracks.forEach(
                        publication => detachTrack(publication.track)))
                .on('participantConnected', participant => add(participant))
                .on('participantDisconnected', participant => remove(participant))
                .on('dominantSpeakerChanged', dominantSpeaker => loudest(dominantSpeaker));
        }
    } catch (error) {
        console.error(`Unable to connect to Room: ${error.message}`);
    }

    return !!window.activeRoom;
}

function initialize(participants) {
    _participants = participants;
    if (_participants) {
        _participants.forEach(participant => registerParticipantEvents(participant));
    }
}

function add(participant) {
    if (_participants && participant) {
        _participants.set(participant.sid, participant);
        registerParticipantEvents(participant);
    }
}

function remove(participant) {
    if (_participants && _participants.has(participant.sid)) {
        participant.tracks.forEach(publication => {
            if (publication.track) {
                detachTrack(publication.track);
            }
        });
        _participants.delete(participant.sid);
    }
}

function loudest(participant) {
    _dominantSpeaker = participant;
}

function registerParticipantEvents(participant) {
    if (participant) {
        participant.tracks.forEach(publication => subscribe(publication));
        participant.tracks.forEach(publication => {
            if (publication.isSubscribed) {
                handleTrackDisabled(publication.track);
            }
        });
        participant.on('trackPublished', publication => subscribe(publication));
        participant.on('trackUnpublished',
            publication => {
                if (publication && publication.track) {
                    detachRemoteTrack(publication.track);
                }
            });
    }
}

function subscribe(publication) {
    if (isMemberDefined(publication, 'on')) {
        publication.on('subscribed', track => {
            attachTrack(track);
            handleTrackDisabled(track);
        });
        publication.on('unsubscribed', track => detachTrack(track));
    }
}

async function leaveRoom() {
    try {
        if (window.activeRoom) {
            window.activeRoom.disconnect();
            window.activeRoom = null;
        }

        if (_participants) {
            _participants.clear();
        }
    }
    catch (error) {
        console.error(error);
    }
}

window.addEventListener('beforeunload', function (event) {
    if (window.videoInterop && window.videoInterop.leaveRoom) {
        window.videoInterop.leaveRoom();
    }
});

function handleTrackDisabled(track) {
    track.on('disabled', () => {
        const videoElement = document.querySelector(`#camera video`);
        if (videoElement) {
            videoElement.style.display = 'none';
            const avatarElement = document.querySelector('#avatar');
            if (avatarElement) {
                avatarElement.style.display = 'block';
            }
        }
    });
}

window.videoInterop = {
    getAudioDevices,
    setMicrophone,
    createOrJoinRoom,
    leaveRoom,
    toggleMicrophone,
    getVideoDevices,
    startVideo,
    toggleCamera,
};