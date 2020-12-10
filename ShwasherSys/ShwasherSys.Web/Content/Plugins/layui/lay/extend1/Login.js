layui.define([ 'layer', 'jquery', 'form' ], function(exports) {// 提示：模块也可以依赖其它模块，如：layui.define('layer',

	var layer = layui.layer;
	var $ = layui.jquery; // 重点处
	var form = layui.form(); // 获取form模块

	// 自定义验证规则
	form.verify({
		operCd : function(value) {
			if (value.length < 8) {
				return '标题至少得8个字符啊';
			}
		}
	});
	
	// 监听提交
	form.on('submit(login)', function(data) {
		layer.alert(JSON.stringify(data), {
			title : '最终的提交信息'
		});
		return false;
	});

	var obj = {
		hello : function(str) {
			alert('Hello ' + (str || 'test'));
		},
		// 初始化时聚焦
		doLogin : function(href) {
			var index = layer.load(1);
			var operCd = $('#Login_operCd').val();// 用户名
			var operPwd = $('#Login_operPwd').val();// 密码

			if (operCd == '') {
				layer.msg('系统提示', '请输入用户名！');
				layer.close(index);
				return false;
			}

			if (operPwd == '') {
				layer.msg('系统提示', '请输入密码！');
				layer.close(index);
				return false;
			}

			$("#Login_loginForm").submit(function(data) {
				var obj = $.parseJSON(data);
				if (obj && obj.success) {
					layer.close(index);
					window.location.href = href;
				} else {
					layer.close(index);
					changeValidateCode();// 修改验证码
					$('#Login_operPwd').val('');// 密码
					layer.msg('错误提示', obj.msg);
				}
			});

			// $("#Login_loginForm").form('submit', {
			// success : function(data) {
			// var obj = $.parseJSON(data);
			// if (obj && obj.success) {
			// layer.close(index);
			// window.location.href = href;
			// } else {
			// layer.close(index);
			// changeValidateCode();// 修改验证码
			// $('#Login_operPwd').val('');// 密码
			// layer.msg('错误提示', obj.msg);
			// }
			// }
			// });
		}

	};

	// 输出test接口
	exports('Login', obj);
});
