import React, { Component } from 'react';
import { SlideDown } from 'react-slidedown';

import 'react-slidedown/lib/slidedown.css';

export class SlidePane extends Component {
    constructor(props) {
        super(props);
        this.state = {
            open: false,
            icon: 'fa-angle-double-down'
        }
    }

    render() {
        return (
            <React.Fragment>
                <div className='row'>
                    <div className='col'>
                        <SlideDown className='overlay'>
                            {this.state.open ? this.props.children : null}
                        </SlideDown>
                    </div>
                </div>
                <div className='row text-center'>
                    <div className='col'>
                        <button className="btn btn-primary" onClick={this.handleToggle.bind(this)}>
                            {this.props.label} <i className={'fa ' + this.state.icon}></i>
                        </button>                
                    </div>
                </div>
            </React.Fragment>
        );
    }

    handleToggle() {
        var isOpen = !this.state.open;
        var icon = isOpen ? 'fa-angle-double-up' : 'fa-angle-double-down'
        this.setState({ open: isOpen, icon: icon });
    }
}