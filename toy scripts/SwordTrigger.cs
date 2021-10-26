
using UdonSharp;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//sword trigger handles the triggering sparks, smoke, and blood when the sword interacts with other objects

//NOTE: blood triggering does not work

public class SwordTrigger : UdonSharpBehaviour
{
    public ParticleSystem sparks = null;//used when colliding with other pickups (like another sword, or a shield)
    public ParticleSystem smoke = null;//used when colliding with 'default' (like the ground, or some other inert object)
    public ParticleSystem blood = null;//used when colliding with players
    void Start()
    {
        //ParticleRest = sparks.gameObject.transform;
    }

    void OnCollisionEnter(Collision collision)
    {
        //sparks.Play();
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        {
            spawnParticleAtPos(collision.contacts[0].point, sparks);
        }
        else if (!(collision.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            spawnParticleAtPos(collision.contacts[0].point, smoke);
        }
        else
        {
            orientParticleByDirec(collision.contacts[0].normal, blood);//blood should be pointed away from the contact point, not in any random direction
            spawnParticleAtPos(collision.contacts[0].point, blood);
        }

        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.red);
        //}
        //Debug.Log("hit");
    }
    private void OnTriggerEnter(Collider other)//this does not work
    {
        //blood.Play();
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("bleedable"))
        {
            Vector3 dir = (transform.position - other.gameObject.transform.position).normalized;
  
            orientParticleByDirec(-dir, blood);
            spawnParticleAtPos(gameObject.transform.position, blood);
        }
    }

	public override void OnPlayerTriggerEnter(VRCPlayerApi player)//this also does not work
    {
        Vector3 dir = (blood.gameObject.transform.position - player.GetPosition()).normalized;
        //RaycastHit hit;
        //Physics.Raycast(blood.gameObject.transform.position, transform.TransformDirection(dir), out hit, 3);
        Vector3 mid = (transform.position - player.GetPosition()) / 2f + transform.position;

        orientParticleByDirec(-dir, blood);
        spawnParticleAtPos(gameObject.transform.position, blood);
        //spawnParticleAtPos(hit.point, blood);
    }

    public void spawnParticleAtPos(Vector3 pos, ParticleSystem particle)
    {
        //pos = particle.transform.InverseTransformPoint(pos);
        Mathf.Clamp(pos.y, -0.3f, 0.3f);//the particle is not allowed to move away from the bounds of the sword, these bounds are hard coded for now
        if (particle != null)
        {
            particle.gameObject.transform.position = pos;
            particle.Play();
        }
    }
    public void orientParticleByDirec(Vector3 direc, ParticleSystem particle)
    {
        particle.gameObject.transform.rotation = Quaternion.LookRotation(direc);
    }
}
