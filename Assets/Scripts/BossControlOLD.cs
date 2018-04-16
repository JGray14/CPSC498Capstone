using UnityEngine;
using System;
using System.Collections;

public class BossControlOLD : MonoBehaviour {

    private Animator animator;
    private Rigidbody2D body;
    private GameObject player;

    public int health;
    private float speed = 0.05f;
    private float dis = 0.1f;
    public bool isEvade = false;
    private bool die;
    public bool acting;
    public bool moving;
    public bool buffed;
    public bool facingRight;
    public int action;
    public int attackCooldown;
    public int evadeCooldown;
    public int buffCooldown;
    public int iFrames;

    private Color normalColor;
    private Color buffColor;
    private Color hitColor;

    void Start() {
        animator = this.GetComponent<Animator>();
        body = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag( "Player" );

        health = 30;
        acting = false;
        moving = true;
        buffed = false;
        facingRight = true;
        action = 0;
        attackCooldown = 0;
        evadeCooldown = 0;
        buffCooldown = 0;

        normalColor = GetComponent<Renderer>().material.color;
        buffColor = Color.gray;
        hitColor = Color.red;
        animator.SetTrigger( "death" );
    }

    void Update() {
        if ( health < 1 ) {
            die = true;
            animator.ResetTrigger( "idle_1" );
            animator.ResetTrigger( "walk" );
            animator.ResetTrigger( "run" );
        } else {
            Vector2 localScale = this.transform.localScale;
            Vector3 velocity = Vector2.zero;
            Vector2 newPosition = Vector2.zero;

            if ( attackCooldown > 0 ) {
                attackCooldown--;
            }
            if ( evadeCooldown > 0 ) {
                evadeCooldown--;
            }
            if ( buffCooldown > 0 ) {
                buffCooldown--;
            }

            if ( playerNearby() && !die ) {
                animator.ResetTrigger( "death" );

                if ( ( player.transform.position.x < body.transform.position.x ) && !isEvade ) {
                    faceLeft();
                } else {
                    faceRight();
                }

                if ( isEvade ) {
                    speed = 0.01f;
                    dis = 0.2f;
                }

                // 1-x Move towards player and attack
                // (x+1)-y roll away if player is close, turn and attack
                // (y+1)-z buff self ( AOE knockback + damage buff )
                // if health < ~40% randomizer changes

                action = UnityEngine.Random.Range( 1, 50 );
                if ( health > 13 ) {
                    int x = 1;
                    int y = 30;
                    int z = 45;
                    if ( x <= action && action <= y ) {
                        if ( playerClose() ) {
                            StartCoroutine( Attack() );
                            attackCooldown = 20;
                        }
                    } else if ( y < action && action <= z && evadeCooldown <= 0 ) {
                        acting = true;
                        StartCoroutine( Evade() );
                        evadeCooldown = 60;
                    } else if ( z < action && action <= 50 && buffCooldown <= 0 ) {
                        acting = true;
                        StartCoroutine( Buff() );
                        buffCooldown = 120;
                    }
                } else {
                
                }

                if ( moving ) {
                    animator.SetTrigger( "walk" );

                    if ( !facingRight ) {
                        newPosition = body.transform.position + new Vector3( -dis, 0, 0 );
                    } else if ( facingRight ) {
                        newPosition = body.transform.position + new Vector3( dis, 0, 0 );
                    }

                    //this.transform.position = newPosition;
                    //this.transform.localScale = localScale;
                    this.transform.position = Vector3.SmoothDamp( this.transform.position, newPosition, ref velocity, speed );
                }

                if ( ( !acting && !moving ) ) {
                    animator.ResetTrigger( "idle_1" );
                    animator.ResetTrigger( "walk" );
                    animator.ResetTrigger( "run" );
                    animator.SetTrigger( "idle_1" );
                }
            }

            //if ( Input.GetKeyUp( KeyCode.A ) || Input.GetKeyUp( KeyCode.D ) || Input.GetKeyUp( KeyCode.LeftArrow ) || Input.GetKeyUp( KeyCode.RightArrow ) ) {
            //    walkStartTime = 0;
            //    animator.ResetTrigger( "idle_1" );
            //    animator.ResetTrigger( "walk" );
            //    animator.ResetTrigger( "run" );
            //    animator.SetTrigger( "idle_1" );
            //}

            //if ( Input.anyKeyDown ) {
            //    foreach ( KeyCode keyCode in Enum.GetValues( typeof( KeyCode ) ) ) {
            //        if ( Input.GetKeyDown( keyCode ) ) {
            //            if ( keyCode == KeyCode.H ) {
            //                animator.SetTrigger( "skill_1" );
            //            } else if ( keyCode == KeyCode.J ) {
            //                animator.SetTrigger( "skill_2" );
            //            } else if ( keyCode == KeyCode.K ) {
            //                animator.SetTrigger( "skill_3" );
            //            } else if ( keyCode == KeyCode.L ) {
            //                animator.SetTrigger( "idle_2" );
            //            } else if ( keyCode == KeyCode.Space ) {
            //                animator.SetTrigger( "evade_1" );
            //                StartCoroutine( Evade() );

            //            }
            //        }
            //    }
            //}
        }
    }

    void LateUpdate() {
        if ( die ) {
            animator.SetTrigger( "death" );
        }
    }

    void faceRight() {
        if ( !facingRight ) {
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            facingRight = !facingRight;
        }
    }

    void faceLeft() {
        if ( facingRight ) {
            Vector2 localScale = gameObject.transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
            facingRight = !facingRight;
        }
    }

    bool playerNearby() {
        if ( player != null ) {
            float distanceFromPlayer = Vector2.Distance( player.GetComponent<Rigidbody2D>().position, body.position );
            return ( distanceFromPlayer < 10 );
        }
        return ( false );
    }

    bool playerClose() {
        if ( player != null ) {
            float distanceFromPlayer = Vector2.Distance( player.GetComponent<Rigidbody2D>().position, body.position );
            return ( distanceFromPlayer < 5 );
        }
        return ( false );
    }

    public IEnumerator Evade() {
        if ( playerClose() ) {
            yield return new WaitForSeconds( 0.2f );
            isEvade = true;
            if ( player.transform.position.x < body.transform.position.x ) {
                faceRight();
            } else {
                faceLeft();
            }
            yield return new WaitForSeconds( 0.2f );
            isEvade = false;
            StartCoroutine( Attack() );
        } else {
            StartCoroutine( Attack() );
        }
    }

    public IEnumerator Hit() {
        animator.SetTrigger( "hit_2" );
        GetComponent<Renderer>().material.color = hitColor;
        yield return new WaitForSeconds( 0.2f );
        GetComponent<Renderer>().material.color = normalColor;
    }

    public IEnumerator Attack() {
        moving = false;
        animator.SetTrigger( "skill_1" );
        yield return new WaitForSeconds( 1.0f );
        acting = false;
        moving = true;
    }

    public IEnumerator Buff() {
        moving = false;
        animator.SetTrigger( "skill_2" );
        buffed = true;
        GetComponent<Renderer>().material.color = buffColor;
        yield return new WaitForSeconds( 1.0f );
        acting = false;
        moving = true;
        yield return new WaitForSeconds( 10.0f );
        GetComponent<Renderer>().material.color = normalColor;
        buffed = false;
    }

    private void OnTriggerEnter2D( Collider2D other ) {
        if ( iFrames <= 0 ) {
            if ( other.tag == "PlayerHurtbox" ) {
                health -= 2;
                StartCoroutine( Hit() );
                iFrames = 30;
            } else if ( other.tag == "PlayerProjectileHurtbox" ) {
                health -= 1;
                StartCoroutine( Hit() );
                iFrames = 30;
            }
        }
    }
}
