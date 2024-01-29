const initialiseFaceio = async () => {
    try {
        faceioRef.current = new faceIO('Faceio-Public-id');
        console.log("FaceIO initialized successfully");
    } catch (error) {
        console.log(error);
        handleError(error)
    }
};
useEffect(() => {
    initialiseFaceio();

}, []);

// HANDLE REGISTER 
const handleRegister = async () => {
    try {
        if (!faceioRef.current) {
            console.error("FaceIO instance is not initialized");
            return;
        }

        await faceioRef.current?.enroll({
            userConsent: false,
            locale: "auto",
            payload: { email: `${email}` },
        });
        // here you add you cookie storing logic ...

        toast.success("Successfully Registered user.")

    } catch (error) {
        handleError(error);
        faceioRef.current?.restartSession();
    }
};

// HANDLE LOGIN 
const handleLogin = async () => {
    try {
        const authenticate = await faceioRef.current?.authenticate();
        console.log("User authenticated successfully:", authenticate);
        setUserLogin(authenticate.payload.email);
        // here you add you cookie storing logic ...

    } catch (error) {
        console.log(error);

        handleError(error);
    }
};

// HANDLING ERRORS
function handleError(errCode: any) {
    const fioErrs = faceioRef.current?.fetchAllErrorCodes()!;
    switch (errCode) {
        case fioErrs.PERMISSION_REFUSED:
            toast.info("Access to the Camera stream was denied by the end user");
            break;
        case fioErrs.NO_FACES_DETECTED:
            toast.info(
                "No faces were detected during the enroll or authentication process"
            );
            break;
        case fioErrs.UNRECOGNIZED_FACE:
            toast.info("Unrecognized face on this application's Facial Index");
            break;
        case fioErrs.MANY_FACES:
            toast.info("Two or more faces were detected during the scan process");
            break;
        case fioErrs.FACE_DUPLICATION:
            toast.info(
                "User enrolled previously (facial features already recorded). Cannot enroll again!"
            );
            break;
        case fioErrs.MINORS_NOT_ALLOWED:
            toast.info("Minors are not allowed to enroll on this application!");
            break;
        case fioErrs.PAD_ATTACK:
            toast.info(
                "Presentation (Spoof) Attack (PAD) detected during the scan process"
            );
            break;
        case fioErrs.FACE_MISMATCH:
            toast.info(
                "Calculated Facial Vectors of the user being enrolled do not matches"
            );
            break;
        case fioErrs.WRONG_PIN_CODE:
            toast.info("Wrong PIN code supplied by the user being authenticated");
            break;
        case fioErrs.PROCESSING_ERR:
            toast.info("Server side error");
            break;
        case fioErrs.UNAUTHORIZED:
            toast.info(
                "Your application is not allowed to perform the requested operation (eg. Invalid ID, Blocked, Paused, etc.). Refer to the FACEIO Console for additional information"
            );
            break;
        case fioErrs.TERMS_NOT_ACCEPTED:
            toast.info(
                "Terms & Conditions set out by FACEIO/host application rejected by the end user"
            );
            break;
        case fioErrs.UI_NOT_READY:
            toast.info(
                "The FACEIO Widget could not be (or is being) injected onto the client DOM"
            );
            break;
        case fioErrs.SESSION_EXPIRED:
            toast.info(
                "Client session expired. The first promise was already fulfilled but the host application failed to act accordingly"
            );
            break;
        case fioErrs.TIMEOUT:
            toast.info(
                "Ongoing operation timed out (eg, Camera access permission, ToS accept delay, Face not yet detected, Server Reply, etc.)"
            );
            break;
        case fioErrs.TOO_MANY_REQUESTS:
            toast.info(
                "Widget instantiation requests exceeded for freemium applications. Does not apply for upgraded applications"
            );
            break;
        case fioErrs.EMPTY_ORIGIN:
            toast.info("Origin or Referer HTTP request header is empty or missing");
            break;
        case fioErrs.FORBIDDDEN_ORIGIN:
            toast.info("Domain origin is forbidden from instantiating fio.js");
            break;
        case fioErrs.FORBIDDDEN_COUNTRY:
            toast.info(
                "Country ISO-3166-1 Code is forbidden from instantiating fio.js"
            );
            break;
        case fioErrs.SESSION_IN_PROGRESS:
            toast.info(
                "Another authentication or enrollment session is in progress"
            );
            break;
        case fioErrs.NETWORK_IO:
        default:
            toast.info(
                "Error while establishing network connection with the target FACEIO processing node"
            );

            }break;
    }
}