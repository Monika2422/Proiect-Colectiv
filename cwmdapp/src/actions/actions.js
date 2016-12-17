/**
 * action types
 */

export const ADD_USER = "ADD_USER";

export const DELETE_USER = "DELETE_USER";

export const EDIT_USER = "EDIT_USER";

export const LOGIN = "LOGIN";

export const LOGOUT = "logout";

/**
 * action creators
 */

export function addUser(user) {
    return {
        type: ADD_USER,
        user
    }
}

export function editUser(user) {
    return {
        type: EDIT_USER,
        user
    }
}

export function deleteUser(userId) {
    return {
        type: DELETE_USER,
        userId
    }
}
