import React, {useState, useEffect} from 'react';
import './css/referente.css'
import fotito2 from './Img/referente.png'



const Referentes = ({information}) => {
    
    const {nombre, apellido, ubicacion, email,foto, name} = information;

    let newImg2="https://localhost:44398//referentes/"+name;



  return (
    
            <div className="card my-2" style={{margin:"auto",borderRadius:"25px", width:"350px", height:"400px",backgroundImage:`url(${fotito2})`}}>
              <img className="img mx-auto mt-4" src={newImg2} style={{borderRadius:"100px", width:"130px", height:"130px", objectFit:"cover"}} alt="Card image cap"/>
              <div className="card-body text-center">
                    
                    <p><small>Referente:</small></p>
                    <h4 className="blue-text card-title mb-1" style={{fontSize:"22px"}}>{nombre} {apellido}</h4>
                    <p><small>Ubicacion:</small></p>
                    <p className="blue-text card-title mb-1" style={{fontSize:"15px"}}>{ubicacion}</p>
                    <p><small>Contacto:</small> </p>
                    <p className="blue-text card-title mb-1"style={{fontSize:"15px"}} >{email}</p>
                
               </div>
            </div>
      
    );


}
export default Referentes;