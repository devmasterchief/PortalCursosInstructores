import openSnackbarReducer from './openSnackbarReducer';
import sesionUsuarioReducer from './sesionUsuarioReducer';

export const mainReducer = ({sesionUsuario, openSnackbar}, action) => {
    return {
        sesionUsuario: sesionUsuarioReducer(sesionUsuario, action),
        openSnackbar: openSnackbarReducer(openSnackbar, action)
    }
};