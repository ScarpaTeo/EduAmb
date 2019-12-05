import React, {useState, useEffect} from 'react';
import './css/referente.css'


const Referentes = ({information}) => {
    
    const {nombre, apellido, ubicacion, email,foto, name} = information;

    let newImg2="https://localhost:44398//referentes/"+name;



  return (
    <div className="row tarjeta">
            <div className="col-10 card text-center" style={{width:'400px',height:"400px", borderRadius: "25px"}}>
              <img className="img mt-3" src={newImg2} style={{borderRadius:"100px",width:"130px",height:"130px",objectFit:"cover"}} alt="Card image cap"/>
              <div className="card-body text-center" style={{fontSize:"15px"}}>
                    
                    <p><small>Referente:</small></p>
                    <h4 className="card-title mb-1">{nombre} {apellido}</h4>
                    <p><small>Ubicacion:</small></p>
                    <p className="card-title mb-1">{ubicacion}</p>
                    <p><small>Contacto:</small></p>
                    <p className="card-title mb-1">{email}</p>
                
               </div>
            </div>
    </div>
    );


}
export default Referentes;