import {ADD_USER} from "../actions/actions";


const initialState = {
    users: []
};

function users(state = initialState, action) {
    switch (action.type) {
        case ADD_USER:
            return Object.assign({}, state, {
                users: [
                    ...state.users,
                    {
                        name: action.name,
                        userName: action.userName,
                        email: action.email,
                        department: action.department,
                        role: action.role
                    }
                ]
            });
        default:
            return state;
    }
}






