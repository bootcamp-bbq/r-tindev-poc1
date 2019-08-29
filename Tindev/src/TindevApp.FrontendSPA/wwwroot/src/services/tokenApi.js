const tokenApi = {
    save: (token) => sessionStorage.setItem('token', token),
    load: () => sessionStorage.getItem('token')
}

export default tokenApi;

// const tokenApiFactory = () => {
//     this.save = token => {
//         sessionStorage.set('user_token', token);
//     }
// }

// export default tokenApiFactory();


// const tokenApiFactory = () => {
//     save = token => {
//         sessionStorage.set('user_token', token);
//     },
    
//     load = () => {
//         return sessionStorage.get('user_token');
//     }
// }

// export default tokenApiFactory();