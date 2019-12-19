import React from 'react';


const Evento = ({eventos}) => {

    let titulo = eventos.name.text;
    if(titulo) {
        titulo = titulo.substr(0, 200) + '...';
   }

    let descripcion = eventos.description.text;
    if(descripcion) {
        descripcion = descripcion.substr(0, 250) + '...';
   }

    return (
        
            <div className="card card-columns my-4 mx-5 mx-sm-5 mx-md-2 pb-3 p-2" style={{borderRadius: "25px",maxWidth:"370px"}}>
                {eventos.logo
                    ? <img className="" src={eventos.logo.url} alt="" style={{borderRadius: "25px"}}/>
                    : <img className="" src="https://images.pexels.com/photos/1048033/pexels-photo-1048033.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500" alt="" style={{borderRadius: "25px"}}/>
                }
                <div className="div text-center mx-3 mp-3">
                <h6 className="pt-3 blue-text mx-4 card-title"><strong>{titulo}</strong></h6>
                    
                <p className="pt-3 mx-2 card-text"><small>{descripcion}</small></p>

                    <a href={eventos.url} target='_blank' type="button" className="mx-auto light-info white-text mb-3 btn btn-outline-info btn-rounded waves-effect btn-sm " style={{width: "150px", color: "white", borderRadius: "15px"}}>Mas Info</a >
                </div>

                    
            </div>

           
       
        
    );
  
}

export default Evento;